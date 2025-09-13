using System;
using System.Collections.Generic;

namespace Asterism.Common.Network
{
    /// <summary>
    /// Webページのメタデータを表すクラス
    /// </summary>
    public class WebPageMetadata
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Keywords { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Charset { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public DateTime? LastModified { get; set; }
        public Dictionary<string, string> CustomMetaTags { get; set; } = new();
    }

    /// <summary>
    /// 解析されたWebコンテンツを表すクラス
    /// </summary>
    public class ParsedWebContent
    {
        public string Url { get; set; } = string.Empty;
        public WebPageMetadata Metadata { get; set; } = new();
        public List<string> Links { get; set; } = new();
        public List<string> Images { get; set; } = new();
        public List<string> Headings { get; set; } = new();
        public List<string> Paragraphs { get; set; } = new();
        public string RawHtml { get; set; } = string.Empty;
        public DateTime ParsedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// 特定のHTMLタグから抽出されたデータを表すクラス
    /// </summary>
    public class ExtractedTagData
    {
        public string TagName { get; set; } = string.Empty;
        public string TextContent { get; set; } = string.Empty;
        public Dictionary<string, string> Attributes { get; set; } = new();
        public string InnerHtml { get; set; } = string.Empty;
        public int Position { get; set; }
    }

    /// <summary>
    /// Webコンテンツクローニングの結果を表すクラス
    /// </summary>
    public class WebContentCloneResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public ParsedWebContent Content { get; set; }
        public List<ExtractedTagData> ExtractedData { get; set; } = new();
        public TimeSpan ProcessingTime { get; set; }
        public bool IsDynamic { get; set; } = false;
    }
}