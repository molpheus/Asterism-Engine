using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Asterism.Common.System.DomAnalysis.Models;

namespace UnitTest.System.DomAnalysis
{
    /// <summary>
    /// ParsingConfigのユニットテスト
    /// Unit tests for ParsingConfig
    /// </summary>
    [TestClass]
    public class ParsingConfigTest
    {
        [TestMethod]
        public void CreateDefault_ShouldReturnValidConfiguration()
        {
            // Act
            var config = ParsingConfig.CreateDefault();

            // Assert
            Assert.IsNotNull(config);
            Assert.IsTrue(config.ExtractText);
            Assert.IsTrue(config.ExtractAttributes);
            Assert.IsTrue(config.ExtractLinks);
            Assert.IsTrue(config.ExtractImages);
        }

        [TestMethod]
        public void CreateLightweight_ShouldReturnMinimalConfiguration()
        {
            // Act
            var config = ParsingConfig.CreateLightweight();

            // Assert
            Assert.IsNotNull(config);
            Assert.IsTrue(config.ExtractText);
            // ライトウェイト設定では一部の機能が無効になる可能性がある
            // Some features may be disabled in lightweight configuration
        }

        [TestMethod]
        public void CreateFullFeatured_ShouldReturnComprehensiveConfiguration()
        {
            // Act
            var config = ParsingConfig.CreateFullFeatured();

            // Assert
            Assert.IsNotNull(config);
            Assert.IsTrue(config.ExtractText);
            Assert.IsTrue(config.ExtractAttributes);
            Assert.IsTrue(config.ExtractLinks);
            Assert.IsTrue(config.ExtractImages);
            Assert.IsTrue(config.ExtractMetadata);
            Assert.IsTrue(config.ValidateStructure);
            Assert.IsTrue(config.ProcessScripts);
            Assert.IsTrue(config.ProcessStyles);
        }

        [TestMethod]
        public void MaxDepth_ShouldControlParsingDepth()
        {
            // Arrange
            var config = ParsingConfig.CreateDefault();

            // Act
            config.MaxDepth = 5;

            // Assert
            Assert.AreEqual(5, config.MaxDepth);
            Assert.IsTrue(config.MaxDepth > 0);
        }

        [TestMethod]
        public void IgnoreTags_ShouldAllowTagFiltering()
        {
            // Arrange
            var config = ParsingConfig.CreateDefault();

            // Act
            config.IgnoreTags.Add("script");
            config.IgnoreTags.Add("style");

            // Assert
            Assert.IsTrue(config.IgnoreTags.Contains("script"));
            Assert.IsTrue(config.IgnoreTags.Contains("style"));
            Assert.AreEqual(2, config.IgnoreTags.Count);
        }

        [TestMethod]
        public void AllowedTags_ShouldRestrictParsing()
        {
            // Arrange
            var config = ParsingConfig.CreateDefault();

            // Act
            config.AllowedTags.Add("div");
            config.AllowedTags.Add("p");
            config.AllowedTags.Add("span");

            // Assert
            Assert.IsTrue(config.AllowedTags.Contains("div"));
            Assert.IsTrue(config.AllowedTags.Contains("p"));
            Assert.IsTrue(config.AllowedTags.Contains("span"));
            Assert.AreEqual(3, config.AllowedTags.Count);
        }

        [TestMethod]
        public void PreserveWhitespace_ShouldControlTextFormatting()
        {
            // Arrange
            var config = ParsingConfig.CreateDefault();

            // Act
            config.PreserveWhitespace = false;

            // Assert
            Assert.IsFalse(config.PreserveWhitespace);
        }

        [TestMethod]
        public void DecodeHtmlEntities_ShouldControlEntityDecoding()
        {
            // Arrange
            var config = ParsingConfig.CreateDefault();

            // Act
            config.DecodeHtmlEntities = true;

            // Assert
            Assert.IsTrue(config.DecodeHtmlEntities);
        }

        [TestMethod]
        public void CustomConfiguration_ShouldAllowFlexibleSetup()
        {
            // Arrange & Act
            var config = new ParsingConfig
            {
                ExtractText = true,
                ExtractAttributes = false,
                ExtractLinks = true,
                ExtractImages = false,
                ExtractMetadata = true,
                ValidateStructure = false,
                ProcessScripts = false,
                ProcessStyles = true,
                MaxDepth = 10,
                PreserveWhitespace = true,
                DecodeHtmlEntities = false
            };

            // Assert
            Assert.IsTrue(config.ExtractText);
            Assert.IsFalse(config.ExtractAttributes);
            Assert.IsTrue(config.ExtractLinks);
            Assert.IsFalse(config.ExtractImages);
            Assert.IsTrue(config.ExtractMetadata);
            Assert.IsFalse(config.ValidateStructure);
            Assert.IsFalse(config.ProcessScripts);
            Assert.IsTrue(config.ProcessStyles);
            Assert.AreEqual(10, config.MaxDepth);
            Assert.IsTrue(config.PreserveWhitespace);
            Assert.IsFalse(config.DecodeHtmlEntities);
        }

        [TestMethod]
        public void DefaultValues_ShouldBeReasonable()
        {
            // Act
            var config = new ParsingConfig();

            // Assert
            Assert.IsNotNull(config.IgnoreTags);
            Assert.IsNotNull(config.AllowedTags);
            Assert.IsTrue(config.MaxDepth > 0);
            // デフォルト値が妥当な範囲内であることを確認
            // Verify that default values are within reasonable ranges
        }

        [TestMethod]
        public void ConfigurationComparison_ShouldShowDifferences()
        {
            // Arrange
            var defaultConfig = ParsingConfig.CreateDefault();
            var lightweightConfig = ParsingConfig.CreateLightweight();
            var fullConfig = ParsingConfig.CreateFullFeatured();

            // Act & Assert
            // 各設定の違いを確認
            // Verify differences between configurations
            
            // デフォルトとライトウェイトの比較
            // Compare default and lightweight
            Assert.IsTrue(defaultConfig.ExtractText == lightweightConfig.ExtractText);
            
            // デフォルトとフル機能の比較
            // Compare default and full-featured
            Assert.IsTrue(defaultConfig.ExtractText == fullConfig.ExtractText);
            Assert.IsTrue(fullConfig.ExtractMetadata);
            Assert.IsTrue(fullConfig.ValidateStructure);
        }

        [TestMethod]
        public void CloneConfiguration_ShouldCreateIndependentCopy()
        {
            // Arrange
            var original = ParsingConfig.CreateDefault();
            original.MaxDepth = 5;
            original.IgnoreTags.Add("script");

            // Act
            var clone = new ParsingConfig
            {
                ExtractText = original.ExtractText,
                ExtractAttributes = original.ExtractAttributes,
                ExtractLinks = original.ExtractLinks,
                ExtractImages = original.ExtractImages,
                ExtractMetadata = original.ExtractMetadata,
                ValidateStructure = original.ValidateStructure,
                ProcessScripts = original.ProcessScripts,
                ProcessStyles = original.ProcessStyles,
                MaxDepth = original.MaxDepth,
                PreserveWhitespace = original.PreserveWhitespace,
                DecodeHtmlEntities = original.DecodeHtmlEntities
            };
            
            // IgnoreTagsを個別にコピー
            foreach (var tag in original.IgnoreTags)
            {
                clone.IgnoreTags.Add(tag);
            }

            // 元の設定を変更
            original.MaxDepth = 10;
            original.IgnoreTags.Add("style");

            // Assert
            Assert.AreEqual(5, clone.MaxDepth);
            Assert.AreEqual(10, original.MaxDepth);
            Assert.AreEqual(1, clone.IgnoreTags.Count);
            Assert.AreEqual(2, original.IgnoreTags.Count);
        }
    }
}