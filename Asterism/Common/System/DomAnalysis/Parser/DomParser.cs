using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Asterism.Common.System.DomAnalysis.Models;

namespace Asterism.Common.System.DomAnalysis.Parser
{
    /// <summary>
    /// DOM解析エンジン - HTMLコンテンツをDOMツリーに変換
    /// DOM parsing engine - converts HTML content to DOM tree
    /// </summary>
    public class DomParser
    {
        private readonly ParsingConfig _config;
        private readonly Regex _tagRegex;
        private readonly Regex _attributeRegex;
        private readonly Dictionary<string, string> _htmlEntities;

        public DomParser(ParsingConfig config = null)
        {
            _config = config ?? ParsingConfig.CreateDefault();
            
            // タグマッチング用の正規表現
            // Regular expression for tag matching
            _tagRegex = new Regex(@"<(?<closing>/?)(?<tag>\w+)(?<attributes>[^>]*)>", 
                RegexOptions.IgnoreCase | RegexOptions.Compiled);
            
            // 属性マッチング用の正規表現
            // Regular expression for attribute matching
            _attributeRegex = new Regex(@"(?<name>\w+)=(?<quote>[""'])(?<value>.*?)\k<quote>", 
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            // HTMLエンティティマップ
            // HTML entity map
            _htmlEntities = InitializeHtmlEntities();
        }

        /// <summary>
        /// HTMLコンテンツを解析してDOMドキュメントを作成
        /// Parse HTML content and create DOM document
        /// </summary>
        public DomDocument Parse(string htmlContent)
        {
            if (string.IsNullOrWhiteSpace(htmlContent))
                throw new ArgumentException("HTML content cannot be null or empty", nameof(htmlContent));

            var document = new DomDocument
            {
                SourceContent = htmlContent,
                Title = ExtractTitle(htmlContent)
            };

            // メタデータの抽出
            // Extract metadata
            if (_config.ExtractMetadata)
            {
                document.Metadata = ExtractMetadata(htmlContent);
            }

            // DOM要素の解析
            // Parse DOM elements
            var elements = ParseElements(htmlContent);
            document.AllElements = elements;

            // 階層構造の構築
            // Build hierarchical structure
            if (_config.BuildHierarchy)
            {
                document.RootElement = BuildHierarchy(elements);
            }

            // 統計情報の更新
            // Update statistics
            document.UpdateStatistics();

            return document;
        }

        /// <summary>
        /// HTML要素を解析してフラットリストを作成
        /// Parse HTML elements and create flat list
        /// </summary>
        private List<DomElement> ParseElements(string htmlContent)
        {
            var elements = new List<DomElement>();
            var matches = _tagRegex.Matches(htmlContent);
            var position = 0;

            foreach (Match match in matches)
            {
                var isClosing = !string.IsNullOrEmpty(match.Groups["closing"].Value);
                if (isClosing) continue; // 終了タグはスキップ

                var tagName = match.Groups["tag"].Value.ToLower();
                
                // タグフィルタリング
                // Tag filtering
                if (!ShouldIncludeTag(tagName)) continue;

                var element = new DomElement
                {
                    TagName = tagName,
                    Position = position++
                };

                // 属性の解析
                // Parse attributes
                if (_config.ExtractAttributes)
                {
                    element.Attributes = ParseAttributes(match.Groups["attributes"].Value);
                }

                // テキストコンテンツの抽出
                // Extract text content
                if (_config.ExtractText)
                {
                    element.InnerText = ExtractTextContent(htmlContent, match, tagName);
                }

                // 内部HTMLの抽出
                // Extract inner HTML
                if (_config.ExtractInnerHtml)
                {
                    element.InnerHtml = ExtractInnerHtml(htmlContent, match, tagName);
                }

                elements.Add(element);

                // 最大要素数チェック
                // Check maximum element count
                if (_config.MaxElements > 0 && elements.Count >= _config.MaxElements)
                    break;
            }

            return elements;
        }

        /// <summary>
        /// タグが解析対象かどうかを判定
        /// Determine if tag should be included in parsing
        /// </summary>
        private bool ShouldIncludeTag(string tagName)
        {
            // 除外タグチェック
            // Check excluded tags
            if (_config.ExcludeTags.Contains(tagName, StringComparer.OrdinalIgnoreCase))
                return false;

            // 対象タグが指定されている場合はそれをチェック
            // Check target tags if specified
            if (_config.TargetTags.Count > 0)
                return _config.TargetTags.Contains(tagName, StringComparer.OrdinalIgnoreCase);

            return true;
        }

        /// <summary>
        /// 属性文字列を解析
        /// Parse attribute string
        /// </summary>
        private Dictionary<string, string> ParseAttributes(string attributeString)
        {
            var attributes = new Dictionary<string, string>();
            var matches = _attributeRegex.Matches(attributeString);

            foreach (Match match in matches)
            {
                var name = match.Groups["name"].Value.ToLower();
                var value = match.Groups["value"].Value;
                
                if (_config.DecodeHtmlEntities)
                {
                    value = DecodeHtmlEntities(value);
                }

                attributes[name] = value;
            }

            return attributes;
        }

        /// <summary>
        /// テキストコンテンツを抽出
        /// Extract text content
        /// </summary>
        private string ExtractTextContent(string htmlContent, Match tagMatch, string tagName)
        {
            // 自己終了タグの場合
            // For self-closing tags
            if (IsSelfClosingTag(tagName))
                return string.Empty;

            try
            {
                var startPos = tagMatch.Index + tagMatch.Length;
                var endTagPattern = $@"</{tagName}>";
                var endMatch = Regex.Match(htmlContent.Substring(startPos), endTagPattern, RegexOptions.IgnoreCase);
                
                if (!endMatch.Success)
                    return string.Empty;

                var content = htmlContent.Substring(startPos, endMatch.Index);
                
                // HTMLタグを除去
                // Remove HTML tags
                content = Regex.Replace(content, @"<[^>]+>", string.Empty);
                
                if (_config.DecodeHtmlEntities)
                {
                    content = DecodeHtmlEntities(content);
                }

                if (_config.NormalizeWhitespace)
                {
                    content = NormalizeWhitespace(content);
                }

                if (_config.IgnoreEmptyText && string.IsNullOrWhiteSpace(content))
                    return string.Empty;

                return content;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 内部HTMLを抽出
        /// Extract inner HTML
        /// </summary>
        private string ExtractInnerHtml(string htmlContent, Match tagMatch, string tagName)
        {
            if (IsSelfClosingTag(tagName))
                return string.Empty;

            try
            {
                var startPos = tagMatch.Index + tagMatch.Length;
                var endTagPattern = $@"</{tagName}>";
                var endMatch = Regex.Match(htmlContent.Substring(startPos), endTagPattern, RegexOptions.IgnoreCase);
                
                return endMatch.Success ? htmlContent.Substring(startPos, endMatch.Index) : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 階層構造を構築
        /// Build hierarchical structure
        /// </summary>
        private DomElement BuildHierarchy(List<DomElement> elements)
        {
            // 簡易的な階層構築（実装は基本的なレベル）
            // Simple hierarchy building (basic level implementation)
            var root = new DomElement
            {
                TagName = "document",
                Position = -1
            };

            foreach (var element in elements)
            {
                element.Parent = root;
                element.Depth = 1;
                root.Children.Add(element);
            }

            return root;
        }

        /// <summary>
        /// タイトルを抽出
        /// Extract title
        /// </summary>
        private string ExtractTitle(string htmlContent)
        {
            var titleMatch = Regex.Match(htmlContent, @"<title[^>]*>(.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (titleMatch.Success)
            {
                var title = titleMatch.Groups[1].Value;
                title = Regex.Replace(title, @"<[^>]+>", string.Empty);
                
                if (_config.DecodeHtmlEntities)
                {
                    title = DecodeHtmlEntities(title);
                }

                return NormalizeWhitespace(title);
            }

            return string.Empty;
        }

        /// <summary>
        /// メタデータを抽出
        /// Extract metadata
        /// </summary>
        private Dictionary<string, string> ExtractMetadata(string htmlContent)
        {
            var metadata = new Dictionary<string, string>();
            var metaMatches = Regex.Matches(htmlContent, @"<meta\s+([^>]+)>", RegexOptions.IgnoreCase);

            foreach (Match match in metaMatches)
            {
                var attributes = ParseAttributes(match.Groups[1].Value);
                
                if (attributes.ContainsKey("name") && attributes.ContainsKey("content"))
                {
                    metadata[attributes["name"]] = attributes["content"];
                }
                else if (attributes.ContainsKey("property") && attributes.ContainsKey("content"))
                {
                    metadata[attributes["property"]] = attributes["content"];
                }
            }

            return metadata;
        }

        /// <summary>
        /// 自己終了タグかどうかを判定
        /// Determine if tag is self-closing
        /// </summary>
        private bool IsSelfClosingTag(string tagName)
        {
            var selfClosingTags = new[] { "img", "br", "hr", "input", "meta", "link", "area", "base", "col", "embed", "source", "track", "wbr" };
            return selfClosingTags.Contains(tagName.ToLower());
        }

        /// <summary>
        /// HTMLエンティティをデコード
        /// Decode HTML entities
        /// </summary>
        private string DecodeHtmlEntities(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            foreach (var entity in _htmlEntities)
            {
                text = text.Replace(entity.Key, entity.Value);
            }

            return text;
        }

        /// <summary>
        /// 空白文字を正規化
        /// Normalize whitespace
        /// </summary>
        private string NormalizeWhitespace(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return Regex.Replace(text.Trim(), @"\s+", " ");
        }

        /// <summary>
        /// HTMLエンティティマップを初期化
        /// Initialize HTML entity map
        /// </summary>
        private Dictionary<string, string> InitializeHtmlEntities()
        {
            return new Dictionary<string, string>
            {
                { "&amp;", "&" },
                { "&lt;", "<" },
                { "&gt;", ">" },
                { "&quot;", "\"" },
                { "&apos;", "'" },
                { "&nbsp;", " " },
                { "&copy;", "©" },
                { "&reg;", "®" },
                { "&trade;", "™" }
            };
        }
    }
}