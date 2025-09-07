using System;
using System.Threading.Tasks;
using Asterism.Common.Network;

namespace Asterism.Examples
{
    /// <summary>
    /// WebContentClonerの使用例を示すクラス
    /// </summary>
    public static class WebContentClonerExample
    {
        /// <summary>
        /// 基本的な使用例
        /// </summary>
        public static async Task BasicUsageExample()
        {
            // WebContentClonerのインスタンスを作成
            var cloner = new WebContentCloner();

            try
            {
                // HTMLサンプル（実際の運用では実際のURLを使用）
                const string sampleHtml = @"
<!DOCTYPE html>
<html lang='ja'>
<head>
    <title>サンプルページ</title>
    <meta name='description' content='これはサンプルページです'>
</head>
<body>
    <h1>メインタイトル</h1>
    <p>これは段落です。</p>
    <a href='https://example.com'>リンク</a>
    <img src='sample.jpg' alt='サンプル画像'>
</body>
</html>";

                // HTMLパーサーを直接使用する例
                var parser = new HtmlParser(sampleHtml);
                
                Console.WriteLine("=== HTMLパーサーの基本使用例 ===");
                
                // タイトルを抽出
                var titles = parser.ExtractTextContent("title");
                Console.WriteLine($"タイトル: {string.Join(", ", titles)}");
                
                // すべてのリンクを抽出
                var links = parser.ExtractAllLinks();
                Console.WriteLine($"リンク: {string.Join(", ", links)}");
                
                // すべての画像を抽出
                var images = parser.ExtractAllImages();
                Console.WriteLine($"画像: {string.Join(", ", images)}");
                
                // H1タグのテキストを抽出
                var headings = parser.ExtractTextContent("h1");
                Console.WriteLine($"見出し: {string.Join(", ", headings)}");
                
                // 段落を抽出
                var paragraphs = parser.ExtractTextContent("p");
                Console.WriteLine($"段落: {string.Join(", ", paragraphs)}");
                
                Console.WriteLine("\n=== 特定の属性値の抽出例 ===");
                
                // 特定の属性値を抽出
                var hrefs = parser.ExtractAttributeValues("a", "href");
                Console.WriteLine($"href属性: {string.Join(", ", hrefs)}");
                
                var imgSrcs = parser.ExtractAttributeValues("img", "src");
                Console.WriteLine($"src属性: {string.Join(", ", imgSrcs)}");
                
                Console.WriteLine("\n=== XPathを使用した抽出例 ===");
                
                // XPathを使用してメタタグの内容を抽出
                var metaDescriptions = parser.ExtractByXPath("//meta[@name='description']");
                Console.WriteLine($"メタ説明: {string.Join(", ", metaDescriptions)}");
                
                Console.WriteLine("\n=== CSSセレクターを使用した抽出例 ===");
                
                // CSSセレクターを使用（基本的なもののみサポート）
                var h1ByClass = parser.ExtractByCssSelector("h1");
                Console.WriteLine($"H1要素: {string.Join(", ", h1ByClass)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 実際のWebサイトからデータを取得する例（注意：実際のWebサイトにアクセスします）
        /// </summary>
        public static async Task RealWebsiteExample()
        {
            var cloner = new WebContentCloner();

            try
            {
                Console.WriteLine("=== 実際のWebサイトからのデータ取得例 ===");
                Console.WriteLine("注意: この例は実際のWebサイトにアクセスするため、ネットワーク接続が必要です。");
                
                // Example.comから基本的な情報を取得
                var result = await cloner.CloneAsync("https://example.com");
                
                if (result.Success && result.Content != null)
                {
                    Console.WriteLine($"URL: {result.Content.Url}");
                    Console.WriteLine($"タイトル: {result.Content.Metadata.Title}");
                    Console.WriteLine($"処理時間: {result.ProcessingTime.TotalMilliseconds}ms");
                    Console.WriteLine($"取得したリンク数: {result.Content.Links.Count}");
                    Console.WriteLine($"取得した画像数: {result.Content.Images.Count}");
                    Console.WriteLine($"抽出されたデータ数: {result.ExtractedData.Count}");
                }
                else
                {
                    Console.WriteLine($"エラー: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// カスタムデータ抽出の例
        /// </summary>
        public static async Task CustomExtractionExample()
        {
            var cloner = new WebContentCloner();

            try
            {
                Console.WriteLine("=== カスタムデータ抽出例 ===");
                
                // 特定のCSSセレクターを使用してデータを抽出
                var cssResults = await cloner.ExtractByCssSelectorAsync("https://example.com", "h1");
                Console.WriteLine($"H1要素: {string.Join(", ", cssResults)}");
                
                // 特定の属性値を抽出
                var linkResults = await cloner.ExtractAttributeValuesAsync("https://example.com", "a", "href");
                Console.WriteLine($"リンク: {string.Join(", ", linkResults)}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラーが発生しました: {ex.Message}");
            }
        }
    }
}