using System;
using System.Linq;
using System.Threading.Tasks;
using Asterism.Common.Network;

namespace Asterism.Common.Examples
{
    /// <summary>
    /// 動的Webコンテンツクローニングの使用例を示すクラス
    /// </summary>
    public class DynamicWebContentExample
    {
        /// <summary>
        /// 基本的な動的コンテンツ抽出の例
        /// </summary>
        public static async Task BasicDynamicExtractionExample()
        {
            Console.WriteLine("=== 基本的な動的コンテンツ抽出 ===");
            
            // 動的コンテンツ抽出のオプション設定
            var options = new DynamicExtractionOptions
            {
                Headless = true,
                PageLoadTimeoutSeconds = 30,
                AdditionalWaitSeconds = 3.0
            };

            var cloner = new WebContentCloner(options);

            try
            {
                // JavaScriptで動的に生成されるコンテンツを含むページを解析
                var result = await cloner.CloneDynamicAsync("https://quotes.toscrape.js.org/");
                
                if (result.Success)
                {
                    Console.WriteLine($"URL: {result.Content.Url}");
                    Console.WriteLine($"タイトル: {result.Content.Metadata.Title}");
                    Console.WriteLine($"処理時間: {result.ProcessingTime.TotalSeconds:F2}秒");
                    Console.WriteLine($"動的コンテンツ: {result.IsDynamic}");
                    Console.WriteLine($"見つかった見出し: {result.Content.Headings.Count}件");
                    Console.WriteLine($"見つかったリンク: {result.Content.Links.Count}件");
                    
                    Console.WriteLine("\n--- 抽出された見出し ---");
                    foreach (var heading in result.Content.Headings.Take(5))
                    {
                        Console.WriteLine($"- {heading}");
                    }
                }
                else
                {
                    Console.WriteLine($"エラー: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"例外が発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 特定の要素を待機する動的コンテンツ抽出の例
        /// </summary>
        public static async Task ElementWaitExtractionExample()
        {
            Console.WriteLine("\n=== 要素待機による動的コンテンツ抽出 ===");
            
            var options = new DynamicExtractionOptions
            {
                Headless = true,
                PageLoadTimeoutSeconds = 30,
                AdditionalWaitSeconds = 2.0
            };

            var cloner = new WebContentCloner(options);

            try
            {
                // 特定の要素（quotes）が表示されるまで待機してから解析
                var result = await cloner.CloneDynamicWithElementWaitAsync(
                    "https://quotes.toscrape.js.org/", 
                    ".quote");
                
                if (result.Success)
                {
                    Console.WriteLine($"URL: {result.Content.Url}");
                    Console.WriteLine($"処理時間: {result.ProcessingTime.TotalSeconds:F2}秒");
                    Console.WriteLine($"見つかった段落: {result.Content.Paragraphs.Count}件");
                    
                    Console.WriteLine("\n--- 抽出された段落 ---");
                    foreach (var paragraph in result.Content.Paragraphs.Take(3))
                    {
                        Console.WriteLine($"- {paragraph}");
                    }
                }
                else
                {
                    Console.WriteLine($"エラー: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"例外が発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 無限スクロールサイトのコンテンツ抽出例
        /// </summary>
        public static async Task InfiniteScrollExtractionExample()
        {
            Console.WriteLine("\n=== 無限スクロールコンテンツ抽出 ===");
            
            var options = new DynamicExtractionOptions
            {
                Headless = true,
                PageLoadTimeoutSeconds = 30,
                AdditionalWaitSeconds = 1.0
            };

            var cloner = new WebContentCloner(options);

            try
            {
                // 無限スクロールページから複数回スクロールしてコンテンツを取得
                var result = await cloner.CloneInfiniteScrollAsync(
                    "https://quotes.toscrape.js.org/scroll", 
                    maxScrolls: 3);
                
                if (result.Success)
                {
                    Console.WriteLine($"URL: {result.Content.Url}");
                    Console.WriteLine($"処理時間: {result.ProcessingTime.TotalSeconds:F2}秒");
                    Console.WriteLine($"見つかった段落: {result.Content.Paragraphs.Count}件");
                    
                    Console.WriteLine("\n--- 抽出された段落（先頭5件） ---");
                    foreach (var paragraph in result.Content.Paragraphs.Take(5))
                    {
                        Console.WriteLine($"- {paragraph.Substring(0, Math.Min(paragraph.Length, 100))}...");
                    }
                }
                else
                {
                    Console.WriteLine($"エラー: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"例外が発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// カスタムJavaScript実行による動的コンテンツ抽出例
        /// </summary>
        public static async Task CustomScriptExtractionExample()
        {
            Console.WriteLine("\n=== カスタムScript実行による動的コンテンツ抽出 ===");
            
            var options = new DynamicExtractionOptions
            {
                Headless = true,
                PageLoadTimeoutSeconds = 30,
                AdditionalWaitSeconds = 2.0
            };

            var cloner = new WebContentCloner(options);

            try
            {
                // ページに特定の操作を実行してからコンテンツを取得
                var customScript = @"
                    // ページ内のボタンをクリック（存在する場合）
                    var button = document.querySelector('.btn-more, .load-more, .next');
                    if (button) {
                        button.click();
                    }
                    
                    // 少し待つ
                    setTimeout(function() {
                        console.log('Custom script executed');
                    }, 1000);
                ";
                
                var result = await cloner.CloneDynamicWithScriptAsync(
                    "https://quotes.toscrape.js.org/", 
                    customScript);
                
                if (result.Success)
                {
                    Console.WriteLine($"URL: {result.Content.Url}");
                    Console.WriteLine($"処理時間: {result.ProcessingTime.TotalSeconds:F2}秒");
                    Console.WriteLine($"見つかった抽出データ: {result.ExtractedData.Count}件");
                    
                    Console.WriteLine("\n--- 抽出されたタグデータ（先頭5件） ---");
                    foreach (var data in result.ExtractedData.Take(5))
                    {
                        Console.WriteLine($"- {data.TagName}: {data.TextContent.Substring(0, Math.Min(data.TextContent.Length, 50))}...");
                    }
                }
                else
                {
                    Console.WriteLine($"エラー: {result.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"例外が発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 静的コンテンツと動的コンテンツの比較例
        /// </summary>
        public static async Task StaticVsDynamicComparisonExample()
        {
            Console.WriteLine("\n=== 静的 vs 動的コンテンツ抽出の比較 ===");
            
            var cloner = new WebContentCloner();
            var dynamicCloner = new WebContentCloner(new DynamicExtractionOptions { Headless = true });

            var testUrl = "https://quotes.toscrape.js.org/";

            try
            {
                // 静的コンテンツ抽出
                var staticResult = await cloner.CloneAsync(testUrl);
                
                // 動的コンテンツ抽出
                var dynamicResult = await dynamicCloner.CloneDynamicAsync(testUrl);

                Console.WriteLine("静的抽出結果:");
                Console.WriteLine($"  成功: {staticResult.Success}");
                Console.WriteLine($"  段落数: {staticResult.Content?.Paragraphs.Count ?? 0}");
                Console.WriteLine($"  処理時間: {staticResult.ProcessingTime.TotalSeconds:F2}秒");

                Console.WriteLine("\n動的抽出結果:");
                Console.WriteLine($"  成功: {dynamicResult.Success}");
                Console.WriteLine($"  段落数: {dynamicResult.Content?.Paragraphs.Count ?? 0}");
                Console.WriteLine($"  処理時間: {dynamicResult.ProcessingTime.TotalSeconds:F2}秒");
                Console.WriteLine($"  動的: {dynamicResult.IsDynamic}");

                var paragraphDifference = (dynamicResult.Content?.Paragraphs.Count ?? 0) - (staticResult.Content?.Paragraphs.Count ?? 0);
                Console.WriteLine($"\n段落数の差: {paragraphDifference} (動的抽出で追加取得)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"例外が発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// すべての例を実行
        /// </summary>
        public static async Task RunAllExamples()
        {
            Console.WriteLine("動的Webコンテンツクローニングの例を実行中...\n");

            await BasicDynamicExtractionExample();
            await ElementWaitExtractionExample();
            await InfiniteScrollExtractionExample();
            await CustomScriptExtractionExample();
            await StaticVsDynamicComparisonExample();

            Console.WriteLine("\n動的Webコンテンツクローニングの例が完了しました。");
        }
    }
}