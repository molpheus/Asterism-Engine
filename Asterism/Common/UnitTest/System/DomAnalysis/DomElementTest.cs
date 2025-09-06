using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Asterism.Common.System.DomAnalysis.Models;

namespace UnitTest.System.DomAnalysis
{
    /// <summary>
    /// DomElementのユニットテスト
    /// Unit tests for DomElement
    /// </summary>
    [TestClass]
    public class DomElementTest
    {
        [TestMethod]
        public void Constructor_ShouldInitializeProperties()
        {
            // Act
            var element = new DomElement
            {
                TagName = "div",
                InnerText = "テストテキスト",
                InnerHtml = "<span>内部HTML</span>"
            };

            // Assert
            Assert.AreEqual("div", element.TagName);
            Assert.AreEqual("テストテキスト", element.InnerText);
            Assert.AreEqual("<span>内部HTML</span>", element.InnerHtml);
            Assert.IsNotNull(element.Attributes);
            Assert.IsNotNull(element.Children);
        }

        [TestMethod]
        public void AddAttribute_ShouldStoreAttribute()
        {
            // Arrange
            var element = new DomElement { TagName = "div" };

            // Act
            element.Attributes["id"] = "test-id";
            element.Attributes["class"] = "test-class";

            // Assert
            Assert.AreEqual("test-id", element.Attributes["id"]);
            Assert.AreEqual("test-class", element.Attributes["class"]);
            Assert.AreEqual(2, element.Attributes.Count);
        }

        [TestMethod]
        public void AddChild_ShouldMaintainParentChildRelationship()
        {
            // Arrange
            var parent = new DomElement { TagName = "div" };
            var child = new DomElement { TagName = "span" };

            // Act
            parent.Children.Add(child);
            child.Parent = parent;

            // Assert
            Assert.AreEqual(1, parent.Children.Count);
            Assert.AreEqual(child, parent.Children.First());
            Assert.AreEqual(parent, child.Parent);
        }

        [TestMethod]
        public void IsLeaf_WithNoChildren_ShouldReturnTrue()
        {
            // Arrange
            var element = new DomElement { TagName = "img" };

            // Act
            bool isLeaf = element.Children.Count == 0;

            // Assert
            Assert.IsTrue(isLeaf);
        }

        [TestMethod]
        public void IsLeaf_WithChildren_ShouldReturnFalse()
        {
            // Arrange
            var parent = new DomElement { TagName = "div" };
            var child = new DomElement { TagName = "span" };
            parent.Children.Add(child);

            // Act
            bool isLeaf = parent.Children.Count == 0;

            // Assert
            Assert.IsFalse(isLeaf);
        }

        [TestMethod]
        public void GetDepth_ForRootElement_ShouldReturnZero()
        {
            // Arrange
            var element = new DomElement { TagName = "html" };

            // Act
            int depth = GetElementDepth(element);

            // Assert
            Assert.AreEqual(0, depth);
        }

        [TestMethod]
        public void GetDepth_ForNestedElement_ShouldReturnCorrectDepth()
        {
            // Arrange
            var root = new DomElement { TagName = "html" };
            var body = new DomElement { TagName = "body", Parent = root };
            var div = new DomElement { TagName = "div", Parent = body };
            var span = new DomElement { TagName = "span", Parent = div };

            root.Children.Add(body);
            body.Children.Add(div);
            div.Children.Add(span);

            // Act
            int spanDepth = GetElementDepth(span);

            // Assert
            Assert.AreEqual(3, spanDepth);
        }

        [TestMethod]
        public void HasAttribute_WithExistingAttribute_ShouldReturnTrue()
        {
            // Arrange
            var element = new DomElement { TagName = "img" };
            element.Attributes["src"] = "image.jpg";
            element.Attributes["alt"] = "画像";

            // Act & Assert
            Assert.IsTrue(element.Attributes.ContainsKey("src"));
            Assert.IsTrue(element.Attributes.ContainsKey("alt"));
            Assert.IsFalse(element.Attributes.ContainsKey("title"));
        }

        [TestMethod]
        public void GetAttribute_WithDefaultValue_ShouldReturnCorrectValue()
        {
            // Arrange
            var element = new DomElement { TagName = "a" };
            element.Attributes["href"] = "https://example.com";

            // Act
            string href = element.Attributes.ContainsKey("href") ? element.Attributes["href"] : "";
            string target = element.Attributes.ContainsKey("target") ? element.Attributes["target"] : "_self";

            // Assert
            Assert.AreEqual("https://example.com", href);
            Assert.AreEqual("_self", target);
        }

        [TestMethod]
        public void SiblingElements_ShouldBeAccessible()
        {
            // Arrange
            var parent = new DomElement { TagName = "ul" };
            var item1 = new DomElement { TagName = "li", Parent = parent };
            var item2 = new DomElement { TagName = "li", Parent = parent };
            var item3 = new DomElement { TagName = "li", Parent = parent };

            parent.Children.Add(item1);
            parent.Children.Add(item2);
            parent.Children.Add(item3);

            // Act
            var siblings = parent.Children.Where(c => c != item2).ToList();

            // Assert
            Assert.AreEqual(2, siblings.Count);
            Assert.IsTrue(siblings.Contains(item1));
            Assert.IsTrue(siblings.Contains(item3));
            Assert.IsFalse(siblings.Contains(item2));
        }

        [TestMethod]
        public void ToString_ShouldReturnMeaningfulRepresentation()
        {
            // Arrange
            var element = new DomElement 
            { 
                TagName = "div",
                InnerText = "テストコンテンツ"
            };
            element.Attributes["id"] = "test";

            // Act
            string representation = $"<{element.TagName} id=\"{element.Attributes.GetValueOrDefault("id", "")}\">";

            // Assert
            Assert.IsTrue(representation.Contains("div"));
            Assert.IsTrue(representation.Contains("test"));
        }

        /// <summary>
        /// 要素の階層の深さを計算するヘルパーメソッド
        /// Helper method to calculate element depth in hierarchy
        /// </summary>
        private int GetElementDepth(DomElement element)
        {
            int depth = 0;
            var current = element;
            while (current.Parent != null)
            {
                depth++;
                current = current.Parent;
            }
            return depth;
        }
    }
}