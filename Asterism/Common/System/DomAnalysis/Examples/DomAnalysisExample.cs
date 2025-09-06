using System;
using System.Collections.Generic;
using Asterism.Common.System.DomAnalysis.Models;
using Asterism.Common.System.DomAnalysis.Parser;
using Asterism.Common.System.DomAnalysis.Analyzers;

namespace Asterism.Common.System.DomAnalysis.Examples
{
    /// <summary>
    /// DOM解析システムの使用例
    /// Usage examples for DOM analysis system
    /// </summary>
    public class DomAnalysisExample
    {
        /// <summary>
        /// 基本的なDOM解析の例
        /// Basic DOM analysis example
        /// </summary>
        public static void BasicParsingExample()
        {
            Console.WriteLine("=== 基本的なDOM解析の例 ===");
            
            // サンプルHTML
            string htmlContent = @"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>テストページ</title>
                    <meta name=""description"" content=""これはテストページです"">
                </head>
                <body>
                    <header>
                        <h1>メインタイトル</h1>
                        <nav>
                            <a href=""#section1"">セクション1</a>
                            <a href=""#section2"">セクション2</a>
                        </nav>
                    </header>
                    <main>
                        <section id=""section1"">
                            <h2>セクション1のタイトル</h2>
                            <p>これはセクション1の内容です。</p>
                            <img src=""image1.jpg"" alt=""画像1"">
                        </section>
                        <section id=""section2"">
                            <h2>セクション2のタイトル</h2>
                            <p>これはセクション2の内容です。</p>
                            <img src=""image2.jpg"">
                        </section>
                    </main>
                    <footer>
                        <p>&copy; 2024 テストサイト</p>
                    </footer>
                </body>
                </html>";

            try
            {
                // パーサーを作成して解析実行
                var config = ParsingConfig.CreateDefault();
                var parser = new DomParser(config);
                var document = parser.Parse(htmlContent);

                // 基本情報の表示
                Console.WriteLine($"ドキュメントタイトル: {document.Title}");
                Console.WriteLine($"総要素数: {document.AllElements.Count}");
                Console.WriteLine($"最大深度: {document.Statistics.MaxDepth}");
                Console.WriteLine($"解析日時: {document.ParsedAt}");

                // メタデータの表示
                Console.WriteLine("\n--- メタデータ ---");
                foreach (var meta in document.Metadata)
                {
                    Console.WriteLine($"{meta.Key}: {meta.Value}");
                }

                // 見出し要素の表示
                Console.WriteLine("\n--- 見出し要素 ---");
                var headings = document.FindElementsByTag("h1");
                headings.AddRange(document.FindElementsByTag("h2"));
                headings.AddRange(document.FindElementsByTag("h3"));
                
                foreach (var heading in headings)
                {
                    Console.WriteLine($"{heading.TagName.ToUpper()}: {heading.InnerText}");
                }

                // リンク要素の表示
                Console.WriteLine("\n--- リンク要素 ---");
                var links = document.FindElementsByTag("a");
                foreach (var link in links)
                {
                    var href = link.GetAttribute("href");
                    Console.WriteLine($"リンク: {link.InnerText} -> {href}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// 構造解析の例
        /// Structure analysis example
        /// </summary>
        public static void StructureAnalysisExample()
        {
            Console.WriteLine("\n=== 構造解析の例 ===");
            
            string htmlContent = @"
                <!DOCTYPE html>
                <html>
                <head>
                    <title>構造解析テスト</title>
                    <meta name=""keywords"" content=""DOM, 解析, HTML"">
                </head>
                <body>
                    <h1>メインタイトル</h1>
                    <h2>サブタイトル1</h2>
                    <h3>サブサブタイトル1-1</h3>
                    <h2>サブタイトル2</h2>
                    <h4>問題のある見出し</h4>
                    
                    <p>通常のパラグラフです。</p>
                    <a href=""https://example.com"">外部リンク</a>
                    <a href=""/internal"">内部リンク</a>
                    <a>空のリンク</a>
                    
                    <img src=""image1.jpg"" alt=""正常な画像"">
                    <img src=""image2.jpg"">
                    
                    <nav>ナビゲーション</nav>
                    <main>メインコンテンツ</main>
                    <aside>サイドバー</aside>
                </body>
                </html>";

            try
            {
                // DOM解析
                var parser = new DomParser(ParsingConfig.CreateDefault());
                var document = parser.Parse(htmlContent);

                // 構造解析実行
                var analyzer = new StructureAnalyzer();
                if (analyzer.CanAnalyze(document))
                {
                    var result = analyzer.Analyze(document);

                    Console.WriteLine($"解析結果: {(result.Success ? "成功" : "失敗")}");
                    Console.WriteLine($"信頼度: {result.Confidence:P}");

                    // 結果データの表示
                    Console.WriteLine("\n--- 解析結果データ ---");
                    foreach (var data in result.Data)
                    {
                        Console.WriteLine($"{data.Key}:");
                        if (data.Value is Dictionary<string, object> dict)
                        {
                            foreach (var item in dict)
                            {
                                Console.WriteLine($"  {item.Key}: {item.Value}");
                            }
                        }
                        else if (data.Value is List<string> list)
                        {
                            foreach (var item in list)
                            {
                                Console.WriteLine($"  - {item}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"  {data.Value}");
                        }
                        Console.WriteLine();
                    }

                    // 警告の表示
                    if (result.Warnings.Count > 0)
                    {
                        Console.WriteLine("--- 警告 ---");
                        foreach (var warning in result.Warnings)
                        {
                            Console.WriteLine($"⚠️ {warning}");
                        }
                    }

                    // エラーの表示
                    if (result.Errors.Count > 0)
                    {
                        Console.WriteLine("--- エラー ---");
                        foreach (var error in result.Errors)
                        {
                            Console.WriteLine($"❌ {error}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("このドキュメントは構造解析できません。");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"エラーが発生しました: {ex.Message}");
            }
        }

        /// <summary>
        /// カスタム設定の例
        /// Custom configuration example
        /// </summary>
        public static void CustomConfigurationExample()
        {
            Console.WriteLine("\n=== カスタム設定の例 ===");
            
            string htmlContent = @"
                <div class=""content"">
                    <h1>タイトル</h1>
                    <p>段落1</p>
                    <p>段落2</p>
                    <span>スパン要素</span>
                    <div>内部div</div>
                </div>";

            // 見出しとパラグラフのみを対象とした軽量設定
            var lightweightConfig = new ParsingConfig
            {
                TargetTags = new List<string> { "h1", "h2", "h3", "p" },
                ExtractText = true,
                ExtractAttributes = false,
                ExtractInnerHtml = false,
                MaxElements = 10,
                IgnoreEmptyText = true,
                BuildHierarchy = false
            };

            Console.WriteLine("軽量設定での解析結果:");
            var parser = new DomParser(lightweightConfig);
            var document = parser.Parse(htmlContent);
            
            Console.WriteLine($"抽出された要素数: {document.AllElements.Count}");
            foreach (var element in document.AllElements)
            {
                Console.WriteLine($"- {element.TagName}: {element.InnerText}");
            }

            // フル機能設定
            var fullConfig = ParsingConfig.CreateFullFeatured();
            Console.WriteLine("\nフル機能設定での解析結果:");
            var fullParser = new DomParser(fullConfig);
            var fullDocument = fullParser.Parse(htmlContent);
            
            Console.WriteLine($"抽出された要素数: {fullDocument.AllElements.Count}");
            foreach (var element in fullDocument.AllElements)
            {
                Console.WriteLine($"- {element.TagName}: {element.InnerText}");
                if (element.Attributes.Count > 0)
                {
                    Console.WriteLine($"  属性: {string.Join(", ", element.Attributes)}");
                }
            }
        }

        /// <summary>
        /// 要素検索の例
        /// Element searching example
        /// </summary>
        public static void ElementSearchingExample()
        {
            Console.WriteLine("\n=== 要素検索の例 ===");
            
            string htmlContent = @"
                <div id=""main"" class=""container"">
                    <h1>メインタイトル</h1>
                    <div class=""content"">
                        <p class=""text"">段落1</p>
                        <p class=""text highlight"">段落2</p>
                    </div>
                    <aside id=""sidebar"">
                        <h2>サイドバー</h2>
                        <ul class=""nav-list"">
                            <li><a href=""#"">リンク1</a></li>
                            <li><a href=""#"">リンク2</a></li>
                        </ul>
                    </aside>
                </div>";

            var parser = new DomParser(ParsingConfig.CreateDefault());
            var document = parser.Parse(htmlContent);

            // タグ名での検索
            Console.WriteLine("=== タグ名での検索 ===");
            var paragraphs = document.FindElementsByTag("p");
            Console.WriteLine($"段落要素数: {paragraphs.Count}");
            foreach (var p in paragraphs)
            {
                Console.WriteLine($"- {p.InnerText}");
            }

            // ID属性での検索
            Console.WriteLine("\n=== ID属性での検索 ===");
            var mainElement = document.FindElementsByAttribute("id", "main");
            Console.WriteLine($"ID='main'の要素数: {mainElement.Count}");
            
            var sidebarElement = document.FindElementsByAttribute("id", "sidebar");
            Console.WriteLine($"ID='sidebar'の要素数: {sidebarElement.Count}");

            // クラス属性での検索
            Console.WriteLine("\n=== クラス属性での検索 ===");
            var textElements = document.FindElementsByAttribute("class");
            Console.WriteLine($"class属性を持つ要素数: {textElements.Count}");
            foreach (var element in textElements)
            {
                Console.WriteLine($"- {element.TagName}: class=\"{element.GetAttribute("class")}\"");
            }

            // CSSセレクター風の検索
            Console.WriteLine("\n=== CSS風セレクター検索 ===");
            var mainById = document.FindElementsBySelector("#main");
            Console.WriteLine($"#main セレクターの結果: {mainById.Count}個");
            
            var textByClass = document.FindElementsBySelector(".text");
            Console.WriteLine($".text セレクターの結果: {textByClass.Count}個");
            
            var h1ByTag = document.FindElementsBySelector("h1");
            Console.WriteLine($"h1 セレクターの結果: {h1ByTag.Count}個");
        }

        /// <summary>
        /// すべての例を実行
        /// Run all examples
        /// </summary>
        public static void RunAllExamples()
        {
            Console.WriteLine("DOM解析システム - 使用例");
            Console.WriteLine("========================\n");

            BasicParsingExample();
            StructureAnalysisExample();
            CustomConfigurationExample();
            ElementSearchingExample();

            Console.WriteLine("\n全ての例の実行が完了しました。");
        }
    }
}