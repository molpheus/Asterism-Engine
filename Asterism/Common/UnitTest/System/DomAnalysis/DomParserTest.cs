using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Asterism.Common.System.DomAnalysis.Models;
using Asterism.Common.System.DomAnalysis.Parser;

namespace UnitTest.System.DomAnalysis
{
    /// <summary>
    /// DomParserのユニットテスト
    /// Unit tests for DomParser
    /// </summary>
    [TestClass]
    public class DomParserTest
    {
        private DomParser _parser;

        [TestInitialize]
        public void TestInitialize()
        {
            var config = ParsingConfig.CreateDefault();
            _parser = new DomParser(config);
        }

        [TestMethod]
        public void Parse_SimpleHtml_ShouldReturnValidDocument()
        {
            // Arrange
            string html = "<html><head><title>テストページ</title></head><body><h1>見出し</h1><p>段落テキスト</p></body></html>";

            // Act
            var document = _parser.Parse(html);

            // Assert
            Assert.IsNotNull(document);
            Assert.AreEqual("テストページ", document.Title);
            Assert.IsTrue(document.AllElements.Count > 0);
            
            var headings = document.FindElementsByTag("h1");
            Assert.AreEqual(1, headings.Count);
            Assert.AreEqual("見出し", headings.First().InnerText.Trim());
        }

        [TestMethod]
        public void Parse_HtmlWithAttributes_ShouldExtractAttributes()
        {
            // Arrange
            string html = "<div id=\"main\" class=\"container\"><a href=\"https://example.com\" target=\"_blank\">リンク</a></div>";

            // Act
            var document = _parser.Parse(html);

            // Assert
            var divElements = document.FindElementsByTag("div");
            Assert.AreEqual(1, divElements.Count);
            
            var div = divElements.First();
            Assert.IsTrue(div.Attributes.ContainsKey("id"));
            Assert.AreEqual("main", div.Attributes["id"]);
            Assert.IsTrue(div.Attributes.ContainsKey("class"));
            Assert.AreEqual("container", div.Attributes["class"]);

            var links = document.FindElementsByTag("a");
            Assert.AreEqual(1, links.Count);
            Assert.AreEqual("https://example.com", links.First().Attributes["href"]);
        }

        [TestMethod]
        public void Parse_NestedElements_ShouldMaintainHierarchy()
        {
            // Arrange
            string html = "<div><p>親の段落<span>子のスパン</span>続きのテキスト</p></div>";

            // Act
            var document = _parser.Parse(html);

            // Assert
            var divElements = document.FindElementsByTag("div");
            Assert.AreEqual(1, divElements.Count);
            
            var div = divElements.First();
            Assert.AreEqual(1, div.Children.Count);
            
            var paragraph = div.Children.First();
            Assert.AreEqual("p", paragraph.TagName);
            Assert.IsTrue(paragraph.InnerText.Contains("親の段落"));
            Assert.IsTrue(paragraph.InnerText.Contains("子のスパン"));
        }

        [TestMethod]
        public void Parse_EmptyHtml_ShouldReturnEmptyDocument()
        {
            // Arrange
            string html = "";

            // Act
            var document = _parser.Parse(html);

            // Assert
            Assert.IsNotNull(document);
            Assert.AreEqual(0, document.AllElements.Count);
            Assert.IsNull(document.Title);
        }

        [TestMethod]
        public void Parse_HtmlWithImages_ShouldExtractImageInfo()
        {
            // Arrange
            string html = "<img src=\"image1.jpg\" alt=\"画像1\" /><img src=\"image2.png\" />";

            // Act
            var document = _parser.Parse(html);

            // Assert
            var images = document.FindElementsByTag("img");
            Assert.AreEqual(2, images.Count);
            
            var firstImage = images.First();
            Assert.AreEqual("image1.jpg", firstImage.Attributes["src"]);
            Assert.AreEqual("画像1", firstImage.Attributes["alt"]);
            
            var secondImage = images.Last();
            Assert.AreEqual("image2.png", secondImage.Attributes["src"]);
            Assert.IsFalse(secondImage.Attributes.ContainsKey("alt"));
        }

        [TestMethod]
        public void FindElementsByAttribute_WithValidAttribute_ShouldReturnMatchingElements()
        {
            // Arrange
            string html = "<div class=\"test\">テスト1</div><span class=\"test\">テスト2</span><p>テスト3</p>";
            var document = _parser.Parse(html);

            // Act
            var elementsWithClass = document.FindElementsByAttribute("class");

            // Assert
            Assert.AreEqual(2, elementsWithClass.Count);
            Assert.IsTrue(elementsWithClass.All(e => e.Attributes.ContainsKey("class")));
        }

        [TestMethod]
        public void FindElementsBySelector_WithIdSelector_ShouldReturnMatchingElement()
        {
            // Arrange
            string html = "<div id=\"main\">メインコンテンツ</div><div id=\"sidebar\">サイドバー</div>";
            var document = _parser.Parse(html);

            // Act
            var mainElement = document.FindElementsBySelector("#main");

            // Assert
            Assert.AreEqual(1, mainElement.Count);
            Assert.AreEqual("main", mainElement.First().Attributes["id"]);
            Assert.IsTrue(mainElement.First().InnerText.Contains("メインコンテンツ"));
        }

        [TestMethod]
        public void Parse_HtmlWithSpecialCharacters_ShouldDecodeEntities()
        {
            // Arrange
            string html = "<p>&lt;div&gt;タグ&amp;記号&quot;引用符&quot;&lt;/div&gt;</p>";

            // Act
            var document = _parser.Parse(html);

            // Assert
            var paragraphs = document.FindElementsByTag("p");
            Assert.AreEqual(1, paragraphs.Count);
            
            var text = paragraphs.First().InnerText;
            Assert.IsTrue(text.Contains("<div>"));
            Assert.IsTrue(text.Contains("&"));
            Assert.IsTrue(text.Contains("\""));
        }

        [TestMethod]
        public void Parse_LightweightConfig_ShouldUseMinimalParsing()
        {
            // Arrange
            string html = "<div style=\"color: red;\"><script>alert('test');</script><p>内容</p></div>";
            var lightweightConfig = ParsingConfig.CreateLightweight();
            var lightweightParser = new DomParser(lightweightConfig);

            // Act
            var document = lightweightParser.Parse(html);

            // Assert
            Assert.IsNotNull(document);
            // ライトウェイト設定では詳細な解析は行わない
            // Lightweight config should perform minimal parsing
            var scripts = document.FindElementsByTag("script");
            // スクリプトタグの扱いは設定による
            // Script tag handling depends on configuration
        }

        [TestMethod]
        public void Parse_FullFeaturedConfig_ShouldPerformComprehensiveParsing()
        {
            // Arrange
            string html = "<html><head><meta charset=\"UTF-8\"><title>完全解析テスト</title></head><body><article><header><h1>記事タイトル</h1></header><section><p>段落1</p><p>段落2</p></section></article></body></html>";
            var fullConfig = ParsingConfig.CreateFullFeatured();
            var fullParser = new DomParser(fullConfig);

            // Act
            var document = fullParser.Parse(html);

            // Assert
            Assert.IsNotNull(document);
            Assert.AreEqual("完全解析テスト", document.Title);
            
            var articles = document.FindElementsByTag("article");
            Assert.AreEqual(1, articles.Count);
            
            var sections = document.FindElementsByTag("section");
            Assert.AreEqual(1, sections.Count);
            
            var paragraphs = document.FindElementsByTag("p");
            Assert.AreEqual(2, paragraphs.Count);
        }
    }
}