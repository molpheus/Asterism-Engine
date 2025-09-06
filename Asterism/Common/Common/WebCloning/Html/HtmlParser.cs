using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Asterism.Common.WebCloning.Models;

namespace Asterism.Common.WebCloning.Html
{
    /// <summary>
    /// Simple HTML parser for extracting specific tags and content
    /// </summary>
    public class HtmlParser
    {
        private readonly ExtractionConfig _config;

        public HtmlParser(ExtractionConfig config = null)
        {
            _config = config ?? new ExtractionConfig();
        }

        /// <summary>
        /// Parse HTML content and extract elements based on configuration
        /// </summary>
        public List<ExtractedElement> ParseHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return new List<ExtractedElement>();

            var elements = new List<ExtractedElement>();
            
            // Extract title if not specifically configured
            if (_config.TargetTags.Count == 0 || _config.TargetTags.Contains("title"))
            {
                var title = ExtractTitle(html);
                if (!string.IsNullOrEmpty(title))
                {
                    elements.Add(new ExtractedElement
                    {
                        TagName = "title",
                        InnerText = title,
                        Position = 0
                    });
                }
            }

            // Extract specific tags
            foreach (var tag in _config.TargetTags)
            {
                if (tag.ToLower() == "title") continue; // Already handled
                
                var tagElements = ExtractElementsByTag(html, tag);
                elements.AddRange(tagElements);
            }

            // If no specific tags configured, extract common important tags
            if (_config.TargetTags.Count == 0)
            {
                var defaultTags = new[] { "h1", "h2", "h3", "p", "a", "img", "div" };
                foreach (var tag in defaultTags)
                {
                    var tagElements = ExtractElementsByTag(html, tag);
                    elements.AddRange(tagElements.Take(10)); // Limit to prevent overwhelming data
                }
            }

            return elements.Take(_config.MaxElements).ToList();
        }

        /// <summary>
        /// Extract title from HTML
        /// </summary>
        private string ExtractTitle(string html)
        {
            var titleMatch = Regex.Match(html, @"<title[^>]*>(.*?)</title>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return titleMatch.Success ? titleMatch.Groups[1].Value.Trim() : string.Empty;
        }

        /// <summary>
        /// Extract elements by specific tag name
        /// </summary>
        private List<ExtractedElement> ExtractElementsByTag(string html, string tagName)
        {
            var elements = new List<ExtractedElement>();
            var position = 0;

            // Pattern to match opening tag, content, and closing tag
            var pattern = $@"<{tagName}([^>]*)>(.*?)</{tagName}>|<{tagName}([^>]*)/>";
            var matches = Regex.Matches(html, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);

            foreach (Match match in matches)
            {
                var element = new ExtractedElement
                {
                    TagName = tagName.ToLower(),
                    Position = position++
                };

                // Extract attributes
                var attributesText = match.Groups[1].Value + match.Groups[3].Value;
                element.Attributes = ParseAttributes(attributesText);

                // Extract content
                if (_config.ExtractText || _config.ExtractHtml)
                {
                    var content = match.Groups[2].Value;
                    
                    if (_config.ExtractText)
                    {
                        element.InnerText = StripHtmlTags(content).Trim();
                    }
                    
                    if (_config.ExtractHtml)
                    {
                        element.InnerHtml = content;
                    }
                }

                elements.Add(element);

                if (elements.Count >= _config.MaxElements)
                    break;
            }

            return elements;
        }

        /// <summary>
        /// Parse HTML attributes from attribute string
        /// </summary>
        private Dictionary<string, string> ParseAttributes(string attributesText)
        {
            var attributes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            
            if (string.IsNullOrWhiteSpace(attributesText))
                return attributes;

            // Pattern to match attribute="value" or attribute='value' or attribute=value
            var pattern = @"(\w+)\s*=\s*[""']([^""']*)[""']|(\w+)\s*=\s*([^\s>]+)";
            var matches = Regex.Matches(attributesText, pattern);

            foreach (Match match in matches)
            {
                string name, value;
                if (!string.IsNullOrEmpty(match.Groups[1].Value))
                {
                    name = match.Groups[1].Value;
                    value = match.Groups[2].Value;
                }
                else
                {
                    name = match.Groups[3].Value;
                    value = match.Groups[4].Value;
                }

                if (!string.IsNullOrEmpty(name))
                {
                    attributes[name] = value ?? string.Empty;
                }
            }

            return attributes;
        }

        /// <summary>
        /// Strip HTML tags from text content
        /// </summary>
        private string StripHtmlTags(string html)
        {
            if (string.IsNullOrEmpty(html))
                return string.Empty;

            // Remove HTML tags
            var stripped = Regex.Replace(html, @"<[^>]+>", string.Empty);
            
            // Decode common HTML entities
            stripped = stripped.Replace("&amp;", "&")
                              .Replace("&lt;", "<")
                              .Replace("&gt;", ">")
                              .Replace("&quot;", "\"")
                              .Replace("&#39;", "'")
                              .Replace("&nbsp;", " ");

            return stripped;
        }

        /// <summary>
        /// Extract metadata like meta tags
        /// </summary>
        public Dictionary<string, string> ExtractMetadata(string html)
        {
            var metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // Extract meta tags
            var metaPattern = @"<meta\s+([^>]+)>";
            var metaMatches = Regex.Matches(html, metaPattern, RegexOptions.IgnoreCase);

            foreach (Match match in metaMatches)
            {
                var attributes = ParseAttributes(match.Groups[1].Value);
                
                if (attributes.ContainsKey("name") && attributes.ContainsKey("content"))
                {
                    metadata[attributes["name"]] = attributes["content"];
                }
                else if (attributes.ContainsKey("property") && attributes.ContainsKey("content"))
                {
                    metadata[attributes["property"]] = attributes["content"];
                }
            }

            return metadata;
        }
    }
}