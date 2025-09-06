using System;
using System.Collections.Generic;

namespace Asterism.Common.System.DomAnalysis.Models
{
    /// <summary>
    /// DOM要素を表現するクラス
    /// Represents a DOM element with its properties and content
    /// </summary>
    public class DomElement
    {
        /// <summary>
        /// タグ名（例: "div", "span", "a"）
        /// Tag name (e.g., "div", "span", "a")
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// 要素のテキストコンテンツ
        /// Text content of the element
        /// </summary>
        public string InnerText { get; set; }

        /// <summary>
        /// 要素の内部HTML
        /// Inner HTML of the element
        /// </summary>
        public string InnerHtml { get; set; }

        /// <summary>
        /// 要素の属性（キー：値）
        /// Element attributes (key: value)
        /// </summary>
        public Dictionary<string, string> Attributes { get; set; }

        /// <summary>
        /// CSSセレクター形式のパス
        /// CSS selector path
        /// </summary>
        public string CssSelector { get; set; }

        /// <summary>
        /// ドキュメント内での位置
        /// Position within the document
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// 親要素への参照
        /// Reference to parent element
        /// </summary>
        public DomElement Parent { get; set; }

        /// <summary>
        /// 子要素のリスト
        /// List of child elements
        /// </summary>
        public List<DomElement> Children { get; set; }

        /// <summary>
        /// 要素の深度レベル
        /// Depth level of the element
        /// </summary>
        public int Depth { get; set; }

        public DomElement()
        {
            Attributes = new Dictionary<string, string>();
            Children = new List<DomElement>();
        }

        /// <summary>
        /// 指定された属性を取得
        /// Get the specified attribute value
        /// </summary>
        public string GetAttribute(string name)
        {
            return Attributes.TryGetValue(name, out var value) ? value : string.Empty;
        }

        /// <summary>
        /// 指定された属性を設定
        /// Set the specified attribute value
        /// </summary>
        public void SetAttribute(string name, string value)
        {
            Attributes[name] = value;
        }

        /// <summary>
        /// 指定されたタグ名の子要素を検索
        /// Find child elements with the specified tag name
        /// </summary>
        public List<DomElement> FindChildrenByTag(string tagName)
        {
            var result = new List<DomElement>();
            FindChildrenByTagRecursive(this, tagName, result);
            return result;
        }

        private void FindChildrenByTagRecursive(DomElement element, string tagName, List<DomElement> result)
        {
            foreach (var child in element.Children)
            {
                if (string.Equals(child.TagName, tagName, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(child);
                }
                FindChildrenByTagRecursive(child, tagName, result);
            }
        }
    }
}