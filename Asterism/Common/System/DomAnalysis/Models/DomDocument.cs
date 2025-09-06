using System;
using System.Collections.Generic;
using System.Linq;

namespace Asterism.Common.System.DomAnalysis.Models
{
    /// <summary>
    /// 解析されたDOMドキュメントを表現するクラス
    /// Represents a parsed DOM document with its structure and metadata
    /// </summary>
    public class DomDocument
    {
        /// <summary>
        /// 元のHTML/XMLソース
        /// Original HTML/XML source
        /// </summary>
        public string SourceContent { get; set; }

        /// <summary>
        /// ドキュメントのルート要素
        /// Root element of the document
        /// </summary>
        public DomElement RootElement { get; set; }

        /// <summary>
        /// ドキュメントのタイトル
        /// Document title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// メタデータ（meta タグからの情報など）
        /// Metadata (information from meta tags, etc.)
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; }

        /// <summary>
        /// 解析された日時
        /// Parse timestamp
        /// </summary>
        public DateTime ParsedAt { get; set; }

        /// <summary>
        /// すべての要素のフラットリスト
        /// Flat list of all elements
        /// </summary>
        public List<DomElement> AllElements { get; set; }

        /// <summary>
        /// ドキュメントの統計情報
        /// Document statistics
        /// </summary>
        public DomStatistics Statistics { get; set; }

        public DomDocument()
        {
            Metadata = new Dictionary<string, string>();
            AllElements = new List<DomElement>();
            ParsedAt = DateTime.Now;
            Statistics = new DomStatistics();
        }

        /// <summary>
        /// 指定されたタグ名の要素を検索
        /// Find elements with the specified tag name
        /// </summary>
        public List<DomElement> FindElementsByTag(string tagName)
        {
            return AllElements.Where(e => string.Equals(e.TagName, tagName, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// 指定された属性を持つ要素を検索
        /// Find elements with the specified attribute
        /// </summary>
        public List<DomElement> FindElementsByAttribute(string attributeName, string attributeValue = null)
        {
            return AllElements.Where(e => 
            {
                if (!e.Attributes.ContainsKey(attributeName))
                    return false;
                
                if (attributeValue == null)
                    return true;
                    
                return string.Equals(e.Attributes[attributeName], attributeValue, StringComparison.OrdinalIgnoreCase);
            }).ToList();
        }

        /// <summary>
        /// CSSセレクター風の検索（簡易版）
        /// CSS selector-like search (simplified version)
        /// </summary>
        public List<DomElement> FindElementsBySelector(string selector)
        {
            var result = new List<DomElement>();
            
            // シンプルなセレクター対応: tag, #id, .class
            // Simple selector support: tag, #id, .class
            if (selector.StartsWith("#"))
            {
                // ID セレクター
                var id = selector.Substring(1);
                result.AddRange(FindElementsByAttribute("id", id));
            }
            else if (selector.StartsWith("."))
            {
                // クラスセレクター
                var className = selector.Substring(1);
                result.AddRange(AllElements.Where(e => 
                {
                    var classAttr = e.GetAttribute("class");
                    return !string.IsNullOrEmpty(classAttr) && 
                           classAttr.Split(' ').Contains(className, StringComparer.OrdinalIgnoreCase);
                }));
            }
            else
            {
                // タグセレクター
                result.AddRange(FindElementsByTag(selector));
            }
            
            return result;
        }

        /// <summary>
        /// ドキュメント統計を更新
        /// Update document statistics
        /// </summary>
        public void UpdateStatistics()
        {
            Statistics.TotalElements = AllElements.Count;
            Statistics.MaxDepth = AllElements.Count > 0 ? AllElements.Max(e => e.Depth) : 0;
            Statistics.TagCounts = AllElements.GroupBy(e => e.TagName.ToLower())
                                            .ToDictionary(g => g.Key, g => g.Count());
        }
    }

    /// <summary>
    /// DOM統計情報
    /// DOM statistics information
    /// </summary>
    public class DomStatistics
    {
        /// <summary>
        /// 総要素数
        /// Total number of elements
        /// </summary>
        public int TotalElements { get; set; }

        /// <summary>
        /// 最大深度
        /// Maximum depth
        /// </summary>
        public int MaxDepth { get; set; }

        /// <summary>
        /// タグ別要素数
        /// Element count by tag
        /// </summary>
        public Dictionary<string, int> TagCounts { get; set; }

        public DomStatistics()
        {
            TagCounts = new Dictionary<string, int>();
        }
    }
}