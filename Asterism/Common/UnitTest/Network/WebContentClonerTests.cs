using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asterism.Common.Network;

namespace UnitTest.Network
{
    [TestClass]
    public class WebContentClonerTests
    {
        private const string TestHtml = @"
<!DOCTYPE html>
<html lang='ja'>
<head>
    <title>テストページ</title>
    <meta name='description' content='テスト用のページです'>
    <meta name='keywords' content='テスト,HTML,パース'>
    <meta charset='UTF-8'>
</head>
<body>
    <h1>メインタイトル</h1>
    <h2 class='subtitle'>サブタイトル</h2>
    <p>これは段落のテストです。</p>
    <p class='highlight'>ハイライトされた段落</p>
    <a href='https://example.com'>リンク1</a>
    <a href='https://test.com'>リンク2</a>
    <img src='image1.jpg' alt='画像1'>
    <img src='image2.png' alt='画像2'>
    <div id='content'>コンテンツエリア</div>
    <span class='info'>情報テキスト</span>
</body>
</html>";

        [TestMethod]
        public void HtmlParser_ExtractTextContent_ShouldReturnCorrectContent()
        {
            // Arrange
            var parser = new HtmlParser(TestHtml);

            // Act
            var titles = parser.ExtractTextContent("title");
            var h1s = parser.ExtractTextContent("h1");
            var paragraphs = parser.ExtractTextContent("p");

            // Assert
            Assert.AreEqual(1, titles.Count);
            Assert.AreEqual("テストページ", titles[0]);
            
            Assert.AreEqual(1, h1s.Count);
            Assert.AreEqual("メインタイトル", h1s[0]);
            
            Assert.AreEqual(2, paragraphs.Count);
            Assert.AreEqual("これは段落のテストです。", paragraphs[0]);
            Assert.AreEqual("ハイライトされた段落", paragraphs[1]);
        }

        [TestMethod]
        public void HtmlParser_ExtractAttributeValues_ShouldReturnCorrectAttributes()
        {
            // Arrange
            var parser = new HtmlParser(TestHtml);

            // Act
            var links = parser.ExtractAttributeValues("a", "href");
            var images = parser.ExtractAttributeValues("img", "src");

            // Assert
            Assert.AreEqual(2, links.Count);
            Assert.IsTrue(links.Contains("https://example.com"));
            Assert.IsTrue(links.Contains("https://test.com"));
            
            Assert.AreEqual(2, images.Count);
            Assert.IsTrue(images.Contains("image1.jpg"));
            Assert.IsTrue(images.Contains("image2.png"));
        }

        [TestMethod]
        public void HtmlParser_ExtractByTagAndClass_ShouldReturnCorrectContent()
        {
            // Arrange
            var parser = new HtmlParser(TestHtml);

            // Act
            var subtitles = parser.ExtractByTagAndClass("h2", "subtitle");
            var highlights = parser.ExtractByTagAndClass("p", "highlight");

            // Assert
            Assert.AreEqual(1, subtitles.Count);
            Assert.AreEqual("サブタイトル", subtitles[0]);
            
            Assert.AreEqual(1, highlights.Count);
            Assert.AreEqual("ハイライトされた段落", highlights[0]);
        }

        [TestMethod]
        public void HtmlParser_ExtractByXPath_ShouldReturnCorrectContent()
        {
            // Arrange
            var parser = new HtmlParser(TestHtml);

            // Act
            var divContent = parser.ExtractByXPath("//div[@id='content']");
            var h1Content = parser.ExtractByXPath("//h1");

            // Assert
            Assert.AreEqual(1, divContent.Count);
            Assert.AreEqual("コンテンツエリア", divContent[0]);
            
            Assert.AreEqual(1, h1Content.Count);
            Assert.AreEqual("メインタイトル", h1Content[0]);
        }

        [TestMethod]
        public void HtmlParser_ExtractByCssSelector_ShouldReturnCorrectContent()
        {
            // Arrange
            var parser = new HtmlParser(TestHtml);

            // Act
            var infoTexts = parser.ExtractByCssSelector(".info");
            var contentDiv = parser.ExtractByCssSelector("#content");

            // Assert
            Assert.AreEqual(1, infoTexts.Count);
            Assert.AreEqual("情報テキスト", infoTexts[0]);
            
            Assert.AreEqual(1, contentDiv.Count);
            Assert.AreEqual("コンテンツエリア", contentDiv[0]);
        }

        [TestMethod]
        public void HtmlParser_ExtractAllLinks_ShouldReturnAllHrefAttributes()
        {
            // Arrange
            var parser = new HtmlParser(TestHtml);

            // Act
            var links = parser.ExtractAllLinks();

            // Assert
            Assert.AreEqual(2, links.Count);
            Assert.IsTrue(links.Contains("https://example.com"));
            Assert.IsTrue(links.Contains("https://test.com"));
        }

        [TestMethod]
        public void HtmlParser_ExtractAllImages_ShouldReturnAllSrcAttributes()
        {
            // Arrange
            var parser = new HtmlParser(TestHtml);

            // Act
            var images = parser.ExtractAllImages();

            // Assert
            Assert.AreEqual(2, images.Count);
            Assert.IsTrue(images.Contains("image1.jpg"));
            Assert.IsTrue(images.Contains("image2.png"));
        }

        [TestMethod]
        public void WebContentModels_ParsedWebContent_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var content = new ParsedWebContent();

            // Assert
            Assert.IsNotNull(content.Metadata);
            Assert.IsNotNull(content.Links);
            Assert.IsNotNull(content.Images);
            Assert.IsNotNull(content.Headings);
            Assert.IsNotNull(content.Paragraphs);
            Assert.AreEqual(string.Empty, content.Url);
            Assert.AreEqual(string.Empty, content.RawHtml);
        }

        [TestMethod]
        public void WebContentModels_WebContentCloneResult_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var result = new WebContentCloneResult();

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual(string.Empty, result.ErrorMessage);
            Assert.IsNotNull(result.ExtractedData);
            Assert.AreEqual(0, result.ExtractedData.Count);
            Assert.IsFalse(result.IsDynamic);
        }

        [TestMethod]
        public void WebContentModels_ExtractedTagData_ShouldInitializeCorrectly()
        {
            // Arrange & Act
            var tagData = new ExtractedTagData();

            // Assert
            Assert.AreEqual(string.Empty, tagData.TagName);
            Assert.AreEqual(string.Empty, tagData.TextContent);
            Assert.AreEqual(string.Empty, tagData.InnerHtml);
            Assert.IsNotNull(tagData.Attributes);
            Assert.AreEqual(0, tagData.Position);
        }

        [TestMethod]
        public void DynamicExtractionOptions_ShouldInitializeWithDefaults()
        {
            // Arrange & Act
            var options = new DynamicExtractionOptions();

            // Assert
            Assert.IsTrue(options.Headless);
            Assert.AreEqual(30, options.PageLoadTimeoutSeconds);
            Assert.AreEqual(2.0, options.AdditionalWaitSeconds);
            Assert.AreEqual(string.Empty, options.UserAgent);
        }

        [TestMethod]
        public void DynamicExtractionOptions_ShouldAllowCustomization()
        {
            // Arrange & Act
            var options = new DynamicExtractionOptions
            {
                Headless = false,
                PageLoadTimeoutSeconds = 60,
                AdditionalWaitSeconds = 5.0,
                UserAgent = "CustomAgent/1.0"
            };

            // Assert
            Assert.IsFalse(options.Headless);
            Assert.AreEqual(60, options.PageLoadTimeoutSeconds);
            Assert.AreEqual(5.0, options.AdditionalWaitSeconds);
            Assert.AreEqual("CustomAgent/1.0", options.UserAgent);
        }

        [TestMethod]
        public void WebContentCloner_ShouldAcceptDynamicOptions()
        {
            // Arrange
            var options = new DynamicExtractionOptions
            {
                Headless = true,
                PageLoadTimeoutSeconds = 45
            };

            // Act & Assert (should not throw)
            var cloner = new WebContentCloner(options);
            Assert.IsNotNull(cloner);
        }

        [TestMethod]
        public void WebContentCloner_ShouldAcceptWebConnecterAndDynamicOptions()
        {
            // Arrange
            var webConnecter = new WebConnecter();
            var options = new DynamicExtractionOptions();

            // Act & Assert (should not throw)
            var cloner = new WebContentCloner(webConnecter, options);
            Assert.IsNotNull(cloner);
        }

        [TestMethod]
        public async Task WebContentCloner_CloneDynamicAsync_ShouldHandleInvalidUrl()
        {
            // Arrange
            var cloner = new WebContentCloner(new DynamicExtractionOptions());
            var invalidUrl = "not-a-valid-url";

            // Act
            var result = await cloner.CloneDynamicAsync(invalidUrl);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.ErrorMessage.Contains("Invalid URL"));
            Assert.IsTrue(result.IsDynamic);
        }

        [TestMethod]
        public async Task WebContentCloner_CloneDynamicWithElementWaitAsync_ShouldHandleInvalidUrl()
        {
            // Arrange
            var cloner = new WebContentCloner(new DynamicExtractionOptions());
            var invalidUrl = "not-a-valid-url";

            // Act
            var result = await cloner.CloneDynamicWithElementWaitAsync(invalidUrl, ".some-element");

            // Assert
            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.ErrorMessage.Contains("Invalid URL"));
            Assert.IsTrue(result.IsDynamic);
        }

        [TestMethod]
        public async Task WebContentCloner_CloneInfiniteScrollAsync_ShouldHandleInvalidUrl()
        {
            // Arrange
            var cloner = new WebContentCloner(new DynamicExtractionOptions());
            var invalidUrl = "not-a-valid-url";

            // Act
            var result = await cloner.CloneInfiniteScrollAsync(invalidUrl, 5);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.ErrorMessage.Contains("Invalid URL"));
            Assert.IsTrue(result.IsDynamic);
        }

        [TestMethod]
        public async Task WebContentCloner_CloneDynamicWithScriptAsync_ShouldHandleInvalidUrl()
        {
            // Arrange
            var cloner = new WebContentCloner(new DynamicExtractionOptions());
            var invalidUrl = "not-a-valid-url";
            var script = "console.log('test');";

            // Act
            var result = await cloner.CloneDynamicWithScriptAsync(invalidUrl, script);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.IsTrue(result.ErrorMessage.Contains("Invalid URL"));
            Assert.IsTrue(result.IsDynamic);
        }
    }
}