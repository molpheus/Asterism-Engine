using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Asterism.Common.Network
{
    /// <summary>
    /// Webコンテンツのクローニングと解析を行うメインクラス
    /// </summary>
    public class WebContentCloner
    {
        private readonly WebConnecter _webConnecter;

        public WebContentCloner() : this(new WebConnecter())
        {
        }

        public WebContentCloner(WebConnecter webConnecter)
        {
            _webConnecter = webConnecter ?? throw new ArgumentNullException(nameof(webConnecter));
        }

        /// <summary>
        /// 指定URLからWebコンテンツを取得し解析する
        /// </summary>
        /// <param name="url">対象URL</param>
        /// <param name="cancellationToken">キャンセレーショントークン</param>
        /// <returns>クローニング結果</returns>
        public async Task<WebContentCloneResult> CloneAsync(string url, CancellationToken cancellationToken = default)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = new WebContentCloneResult();

            try
            {
                // URLバリデーション
                if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                {
                    result.Success = false;
                    result.ErrorMessage = $"Invalid URL: {url}";
                    return result;
                }

                // Webコンテンツの取得
                var htmlContent = await _webConnecter.GetAsync(url, cancellationToken);
                
                // HTMLの解析
                var parser = new HtmlParser(htmlContent);
                var parsedContent = await ParseContentAsync(parser, url, htmlContent);

                result.Success = true;
                result.Content = parsedContent;
                result.ExtractedData = ExtractTagData(parser);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            finally
            {
                stopwatch.Stop();
                result.ProcessingTime = stopwatch.Elapsed;
            }

            return result;
        }

        /// <summary>
        /// HTMLコンテンツを解析してParsedWebContentを作成
        /// </summary>
        private async Task<ParsedWebContent> ParseContentAsync(HtmlParser parser, string url, string htmlContent)
        {
            return await Task.Run(() =>
            {
                var content = new ParsedWebContent
                {
                    Url = url,
                    RawHtml = htmlContent,
                    ParsedAt = DateTime.UtcNow
                };

                // メタデータの抽出
                content.Metadata = ExtractMetadata(parser);

                // リンクと画像の抽出
                content.Links = parser.ExtractAllLinks();
                content.Images = parser.ExtractAllImages();

                // ヘッダーとパラグラフの抽出
                content.Headings = ExtractHeadings(parser);
                content.Paragraphs = parser.ExtractTextContent("p");

                return content;
            });
        }

        /// <summary>
        /// メタデータを抽出
        /// </summary>
        private WebPageMetadata ExtractMetadata(HtmlParser parser)
        {
            var metadata = new WebPageMetadata();

            // タイトルの取得
            var titles = parser.ExtractTextContent("title");
            metadata.Title = titles.FirstOrDefault() ?? string.Empty;

            // メタタグからの情報取得
            var descriptions = parser.ExtractByXPath("//meta[@name='description']/@content");
            metadata.Description = descriptions.FirstOrDefault() ?? string.Empty;

            var keywords = parser.ExtractByXPath("//meta[@name='keywords']/@content");
            metadata.Keywords = keywords.FirstOrDefault() ?? string.Empty;

            var authors = parser.ExtractByXPath("//meta[@name='author']/@content");
            metadata.Author = authors.FirstOrDefault() ?? string.Empty;

            var charsets = parser.ExtractByXPath("//meta[@charset]/@charset");
            metadata.Charset = charsets.FirstOrDefault() ?? string.Empty;

            var languages = parser.ExtractByXPath("//html/@lang");
            metadata.Language = languages.FirstOrDefault() ?? string.Empty;

            return metadata;
        }

        /// <summary>
        /// 見出しタグ（h1-h6）を抽出
        /// </summary>
        private List<string> ExtractHeadings(HtmlParser parser)
        {
            var headings = new List<string>();
            for (int i = 1; i <= 6; i++)
            {
                headings.AddRange(parser.ExtractTextContent($"h{i}"));
            }
            return headings;
        }

        /// <summary>
        /// 特定のタグデータを抽出
        /// </summary>
        private List<ExtractedTagData> ExtractTagData(HtmlParser parser)
        {
            var extractedData = new List<ExtractedTagData>();

            // 重要なタグからデータを抽出
            var importantTags = new[] { "h1", "h2", "h3", "title", "meta", "a", "img" };

            foreach (var tag in importantTags)
            {
                var textContents = parser.ExtractTextContent(tag);
                for (int i = 0; i < textContents.Count; i++)
                {
                    extractedData.Add(new ExtractedTagData
                    {
                        TagName = tag,
                        TextContent = textContents[i],
                        Position = i
                    });
                }
            }

            return extractedData;
        }

        /// <summary>
        /// 特定のタグから特定の属性値を抽出する便利メソッド
        /// </summary>
        /// <param name="url">対象URL</param>
        /// <param name="tagName">タグ名</param>
        /// <param name="attributeName">属性名</param>
        /// <param name="cancellationToken">キャンセレーショントークン</param>
        /// <returns>抽出された属性値のリスト</returns>
        public async Task<List<string>> ExtractAttributeValuesAsync(string url, string tagName, string attributeName, CancellationToken cancellationToken = default)
        {
            try
            {
                var htmlContent = await _webConnecter.GetAsync(url, cancellationToken);
                var parser = new HtmlParser(htmlContent);
                return parser.ExtractAttributeValues(tagName, attributeName);
            }
            catch
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// 特定のCSSセレクターに一致する要素のテキストを抽出する便利メソッド
        /// </summary>
        /// <param name="url">対象URL</param>
        /// <param name="cssSelector">CSSセレクター</param>
        /// <param name="cancellationToken">キャンセレーショントークン</param>
        /// <returns>抽出されたテキストのリスト</returns>
        public async Task<List<string>> ExtractByCssSelectorAsync(string url, string cssSelector, CancellationToken cancellationToken = default)
        {
            try
            {
                var htmlContent = await _webConnecter.GetAsync(url, cancellationToken);
                var parser = new HtmlParser(htmlContent);
                return parser.ExtractByCssSelector(cssSelector);
            }
            catch
            {
                return new List<string>();
            }
        }
    }
}