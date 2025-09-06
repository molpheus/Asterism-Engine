using System;
using System.Collections.Generic;

namespace Asterism.Common.WebCloning.Models
{
    /// <summary>
    /// Represents extracted web content with parsed HTML data
    /// </summary>
    public class WebContent
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string RawHtml { get; set; }
        public DateTime FetchedAt { get; set; }
        public List<ExtractedElement> Elements { get; set; }
        public Dictionary<string, string> Metadata { get; set; }

        public WebContent()
        {
            Elements = new List<ExtractedElement>();
            Metadata = new Dictionary<string, string>();
            FetchedAt = DateTime.Now;
        }
    }

    /// <summary>
    /// Represents a parsed HTML element with extracted data
    /// </summary>
    public class ExtractedElement
    {
        public string TagName { get; set; }
        public string InnerText { get; set; }
        public string InnerHtml { get; set; }
        public Dictionary<string, string> Attributes { get; set; }
        public string CssSelector { get; set; }
        public int Position { get; set; }

        public ExtractedElement()
        {
            Attributes = new Dictionary<string, string>();
        }
    }

    /// <summary>
    /// Configuration for web content extraction
    /// </summary>
    public class ExtractionConfig
    {
        public List<string> TargetTags { get; set; }
        public List<string> CssSelectors { get; set; }
        public List<string> RequiredAttributes { get; set; }
        public bool ExtractText { get; set; }
        public bool ExtractHtml { get; set; }
        public bool ExtractAttributes { get; set; }
        public int MaxElements { get; set; }

        public ExtractionConfig()
        {
            TargetTags = new List<string>();
            CssSelectors = new List<string>();
            RequiredAttributes = new List<string>();
            ExtractText = true;
            ExtractHtml = false;
            ExtractAttributes = true;
            MaxElements = 1000;
        }
    }
}