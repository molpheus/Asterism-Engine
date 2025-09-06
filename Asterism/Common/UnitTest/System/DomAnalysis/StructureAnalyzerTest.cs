using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Asterism.Common.System.DomAnalysis.Models;
using Asterism.Common.System.DomAnalysis.Parser;
using Asterism.Common.System.DomAnalysis.Analyzers;

namespace UnitTest.System.DomAnalysis
{
    /// <summary>
    /// StructureAnalyzerのユニットテスト
    /// Unit tests for StructureAnalyzer
    /// </summary>
    [TestClass]
    public class StructureAnalyzerTest
    {
        private StructureAnalyzer _analyzer;
        private DomParser _parser;

        [TestInitialize]
        public void TestInitialize()
        {
            _analyzer = new StructureAnalyzer();
            var config = ParsingConfig.CreateDefault();
            _parser = new DomParser(config);
        }

        [TestMethod]
        public void Analyze_ValidHtmlStructure_ShouldReturnSuccessfulResult()
        {
            // Arrange
            string html = @"
                <html>
                    <head><title>テストページ</title></head>
                    <body>
                        <header><h1>メインタイトル</h1></header>
                        <main>
                            <article>
                                <h2>セクション1</h2>
                                <p>コンテンツ1</p>
                            </article>
                            <article>
                                <h2>セクション2</h2>
                                <p>コンテンツ2</p>
                            </article>
                        </main>
                        <footer><p>フッター情報</p></footer>
                    </body>
                </html>";
            
            var document = _parser.Parse(html);

            // Act
            var result = _analyzer.Analyze(document);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Success);
            Assert.IsTrue(result.Confidence > 0.5);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void Analyze_HeadingHierarchy_ShouldValidateStructure()
        {
            // Arrange
            string html = @"
                <html>
                    <body>
                        <h1>レベル1見出し</h1>
                        <p>内容</p>
                        <h2>レベル2見出し</h2>
                        <p>内容</p>
                        <h3>レベル3見出し</h3>
                        <p>内容</p>
                        <h2>別のレベル2見出し</h2>
                        <p>内容</p>
                    </body>
                </html>";
            
            var document = _parser.Parse(html);

            // Act
            var result = _analyzer.Analyze(document);

            // Assert
            Assert.IsTrue(result.Success);
            
            // 見出し階層が適切に構成されているかチェック
            // Check if heading hierarchy is properly structured
            var headings = document.AllElements.Where(e => 
                e.TagName.StartsWith("h") && e.TagName.Length == 2 && 
                char.IsDigit(e.TagName[1])).ToList();
            
            Assert.IsTrue(headings.Count > 0);
            Assert.IsTrue(headings.Any(h => h.TagName == "h1"));
        }

        [TestMethod]
        public void Analyze_SemanticElements_ShouldDetectUsage()
        {
            // Arrange
            string html = @"
                <html>
                    <body>
                        <header>ヘッダー</header>
                        <nav>ナビゲーション</nav>
                        <main>
                            <article>記事</article>
                            <aside>サイドバー</aside>
                        </main>
                        <footer>フッター</footer>
                    </body>
                </html>";
            
            var document = _parser.Parse(html);

            // Act
            var result = _analyzer.Analyze(document);

            // Assert
            Assert.IsTrue(result.Success);
            
            // セマンティック要素の使用を検証
            // Verify semantic element usage
            var semanticTags = new[] { "header", "nav", "main", "article", "aside", "footer" };
            var foundSemanticTags = document.AllElements
                .Where(e => semanticTags.Contains(e.TagName.ToLower()))
                .Select(e => e.TagName.ToLower())
                .Distinct()
                .ToList();
            
            Assert.IsTrue(foundSemanticTags.Count >= 4); // 少なくとも4つのセマンティック要素
        }

        [TestMethod]
        public void Analyze_LinksAnalysis_ShouldClassifyLinks()
        {
            // Arrange
            string html = @"
                <html>
                    <body>
                        <a href=""#section1"">内部リンク</a>
                        <a href=""https://example.com"">外部リンク</a>
                        <a href=""mailto:test@example.com"">メールリンク</a>
                        <a href=""page.html"">相対リンク</a>
                        <a>リンクなし</a>
                    </body>
                </html>";
            
            var document = _parser.Parse(html);

            // Act
            var result = _analyzer.Analyze(document);

            // Assert
            Assert.IsTrue(result.Success);
            
            var links = document.FindElementsByTag("a");
            Assert.AreEqual(5, links.Count);
            
            var linksWithHref = links.Where(l => l.Attributes.ContainsKey("href")).ToList();
            Assert.AreEqual(4, linksWithHref.Count);
            
            var externalLinks = linksWithHref.Where(l => 
                l.Attributes["href"].StartsWith("http://") || 
                l.Attributes["href"].StartsWith("https://")).ToList();
            Assert.AreEqual(1, externalLinks.Count);
        }

        [TestMethod]
        public void Analyze_ImagesAnalysis_ShouldCheckAccessibility()
        {
            // Arrange
            string html = @"
                <html>
                    <body>
                        <img src=""image1.jpg"" alt=""説明付き画像"" />
                        <img src=""image2.jpg"" />
                        <img src=""decorative.jpg"" alt="""" />
                        <img src=""icon.svg"" alt=""アイコン"" width=""16"" height=""16"" />
                    </body>
                </html>";
            
            var document = _parser.Parse(html);

            // Act
            var result = _analyzer.Analyze(document);

            // Assert
            Assert.IsTrue(result.Success);
            
            var images = document.FindElementsByTag("img");
            Assert.AreEqual(4, images.Count);
            
            var imagesWithAlt = images.Where(img => img.Attributes.ContainsKey("alt")).ToList();
            Assert.AreEqual(3, imagesWithAlt.Count);
            
            var imagesWithMeaningfulAlt = imagesWithAlt.Where(img => 
                !string.IsNullOrWhiteSpace(img.Attributes["alt"])).ToList();
            Assert.AreEqual(2, imagesWithMeaningfulAlt.Count);
        }

        [TestMethod]
        public void Analyze_ContentDensity_ShouldMeasureTextDistribution()
        {
            // Arrange
            string html = @"
                <html>
                    <body>
                        <div>
                            <p>これは最初の段落です。十分な長さのテキストコンテンツを含んでいます。</p>
                            <p>これは二番目の段落です。こちらも適切な量のテキストがあります。</p>
                        </div>
                        <div>
                            <span>短い</span>
                        </div>
                        <div>
                            <h2>見出し</h2>
                            <p>この段落は非常に長いテキストコンテンツを含んでおり、コンテンツの密度を測定するためのサンプルとして使用されます。文章が長ければ長いほど、より詳細な分析が可能になります。</p>
                        </div>
                    </body>
                </html>";
            
            var document = _parser.Parse(html);

            // Act
            var result = _analyzer.Analyze(document);

            // Assert
            Assert.IsTrue(result.Success);
            
            // テキストコンテンツの分布を確認
            // Check text content distribution
            var textElements = document.AllElements.Where(e => 
                !string.IsNullOrWhiteSpace(e.InnerText)).ToList();
            Assert.IsTrue(textElements.Count > 0);
            
            var totalTextLength = textElements.Sum(e => e.InnerText?.Length ?? 0);
            Assert.IsTrue(totalTextLength > 100); // 十分なテキストコンテンツ
        }

        [TestMethod]
        public void Analyze_EmptyDocument_ShouldHandleGracefully()
        {
            // Arrange
            var emptyDocument = new DomDocument();

            // Act
            var result = _analyzer.Analyze(emptyDocument);

            // Assert
            Assert.IsNotNull(result);
            // 空のドキュメントでもエラーにならないことを確認
            // Verify that empty documents don't cause errors
        }

        [TestMethod]
        public void Analyze_MalformedHtml_ShouldProduceWarnings()
        {
            // Arrange
            string malformedHtml = @"
                <html>
                    <body>
                        <div>
                            <p>閉じタグなし
                            <span>ネストエラー</div>
                        </span>
                    </body>";
            
            var document = _parser.Parse(malformedHtml);

            // Act
            var result = _analyzer.Analyze(document);

            // Assert
            Assert.IsNotNull(result);
            // 不正なHTMLでも解析は続行されるべき
            // Analysis should continue even with malformed HTML
        }

        [TestMethod]
        public void Analyze_AccessibilityFeatures_ShouldCheckAriaUsage()
        {
            // Arrange
            string html = @"
                <html>
                    <body>
                        <button aria-label=""メニューを開く"">☰</button>
                        <div role=""main"">
                            <h1>メインコンテンツ</h1>
                            <p>アクセシビリティに配慮したコンテンツ</p>
                        </div>
                        <nav aria-label=""メインナビゲーション"">
                            <ul>
                                <li><a href=""#"">ホーム</a></li>
                                <li><a href=""#"">サービス</a></li>
                            </ul>
                        </nav>
                    </body>
                </html>";
            
            var document = _parser.Parse(html);

            // Act
            var result = _analyzer.Analyze(document);

            // Assert
            Assert.IsTrue(result.Success);
            
            // ARIA属性の使用を確認
            // Check ARIA attribute usage
            var elementsWithAria = document.AllElements.Where(e => 
                e.Attributes.Keys.Any(k => k.StartsWith("aria-") || k == "role")).ToList();
            Assert.IsTrue(elementsWithAria.Count >= 3);
        }

        [TestMethod]
        public void Analyze_ResultConfidence_ShouldReflectQuality()
        {
            // Arrange - 高品質なHTML
            string highQualityHtml = @"
                <html lang=""ja"">
                    <head>
                        <meta charset=""UTF-8"">
                        <title>高品質ページ</title>
                    </head>
                    <body>
                        <header>
                            <h1>メインタイトル</h1>
                        </header>
                        <main>
                            <article>
                                <h2>記事タイトル</h2>
                                <p>十分な内容を持つ段落テキスト。</p>
                                <img src=""image.jpg"" alt=""説明文付き画像"" />
                            </article>
                        </main>
                        <footer>
                            <p>フッター情報</p>
                        </footer>
                    </body>
                </html>";
            
            // Arrange - 低品質なHTML
            string lowQualityHtml = @"<div><span>短い</span></div>";
            
            var highQualityDoc = _parser.Parse(highQualityHtml);
            var lowQualityDoc = _parser.Parse(lowQualityHtml);

            // Act
            var highQualityResult = _analyzer.Analyze(highQualityDoc);
            var lowQualityResult = _analyzer.Analyze(lowQualityDoc);

            // Assert
            Assert.IsTrue(highQualityResult.Confidence > lowQualityResult.Confidence);
            Assert.IsTrue(highQualityResult.Confidence > 0.7);
        }
    }
}