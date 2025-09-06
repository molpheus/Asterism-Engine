using System.Collections.Generic;

namespace Asterism.Common.System.DomAnalysis.Models
{
    /// <summary>
    /// DOM解析の設定クラス
    /// Configuration class for DOM parsing
    /// </summary>
    public class ParsingConfig
    {
        /// <summary>
        /// 解析対象のタグ名リスト（空の場合は全タグ）
        /// List of target tag names (all tags if empty)
        /// </summary>
        public List<string> TargetTags { get; set; }

        /// <summary>
        /// 解析対象のCSSセレクター
        /// Target CSS selectors for parsing
        /// </summary>
        public List<string> CssSelectors { get; set; }

        /// <summary>
        /// テキストコンテンツを抽出するかどうか
        /// Whether to extract text content
        /// </summary>
        public bool ExtractText { get; set; }

        /// <summary>
        /// 属性を抽出するかどうか
        /// Whether to extract attributes
        /// </summary>
        public bool ExtractAttributes { get; set; }

        /// <summary>
        /// 内部HTMLを抽出するかどうか
        /// Whether to extract inner HTML
        /// </summary>
        public bool ExtractInnerHtml { get; set; }

        /// <summary>
        /// 最大抽出要素数（0は無制限）
        /// Maximum number of elements to extract (0 = unlimited)
        /// </summary>
        public int MaxElements { get; set; }

        /// <summary>
        /// 最大深度レベル（0は無制限）
        /// Maximum depth level (0 = unlimited)
        /// </summary>
        public int MaxDepth { get; set; }

        /// <summary>
        /// 除外するタグ名リスト
        /// List of tag names to exclude
        /// </summary>
        public List<string> ExcludeTags { get; set; }

        /// <summary>
        /// 空のテキストノードを無視するかどうか
        /// Whether to ignore empty text nodes
        /// </summary>
        public bool IgnoreEmptyText { get; set; }

        /// <summary>
        /// HTMLエンティティをデコードするかどうか
        /// Whether to decode HTML entities
        /// </summary>
        public bool DecodeHtmlEntities { get; set; }

        /// <summary>
        /// 空白文字を正規化するかどうか
        /// Whether to normalize whitespace
        /// </summary>
        public bool NormalizeWhitespace { get; set; }

        /// <summary>
        /// メタデータを抽出するかどうか
        /// Whether to extract metadata
        /// </summary>
        public bool ExtractMetadata { get; set; }

        /// <summary>
        /// 階層構造を構築するかどうか
        /// Whether to build hierarchical structure
        /// </summary>
        public bool BuildHierarchy { get; set; }

        public ParsingConfig()
        {
            TargetTags = new List<string>();
            CssSelectors = new List<string>();
            ExcludeTags = new List<string>();
            ExtractText = true;
            ExtractAttributes = true;
            ExtractInnerHtml = false;
            MaxElements = 0;
            MaxDepth = 0;
            IgnoreEmptyText = true;
            DecodeHtmlEntities = true;
            NormalizeWhitespace = true;
            ExtractMetadata = true;
            BuildHierarchy = true;
        }

        /// <summary>
        /// デフォルト設定を作成
        /// Create default configuration
        /// </summary>
        public static ParsingConfig CreateDefault()
        {
            return new ParsingConfig
            {
                TargetTags = new List<string> { "h1", "h2", "h3", "h4", "h5", "h6", "p", "a", "img", "div", "span" },
                ExtractText = true,
                ExtractAttributes = true,
                ExtractMetadata = true,
                IgnoreEmptyText = true,
                DecodeHtmlEntities = true,
                NormalizeWhitespace = true,
                BuildHierarchy = true
            };
        }

        /// <summary>
        /// 軽量設定を作成（テキストのみ）
        /// Create lightweight configuration (text only)
        /// </summary>
        public static ParsingConfig CreateLightweight()
        {
            return new ParsingConfig
            {
                TargetTags = new List<string> { "h1", "h2", "h3", "p", "a" },
                ExtractText = true,
                ExtractAttributes = false,
                ExtractInnerHtml = false,
                ExtractMetadata = false,
                IgnoreEmptyText = true,
                MaxElements = 100,
                BuildHierarchy = false
            };
        }

        /// <summary>
        /// フル機能設定を作成
        /// Create full-featured configuration
        /// </summary>
        public static ParsingConfig CreateFullFeatured()
        {
            return new ParsingConfig
            {
                ExtractText = true,
                ExtractAttributes = true,
                ExtractInnerHtml = true,
                ExtractMetadata = true,
                IgnoreEmptyText = false,
                DecodeHtmlEntities = true,
                NormalizeWhitespace = true,
                BuildHierarchy = true
            };
        }
    }
}