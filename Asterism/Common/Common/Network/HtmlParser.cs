using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Asterism.Common.Network
{
    /// <summary>
    /// HTMLコンテンツの解析と特定データの抽出を行うクラス
    /// </summary>
    public class HtmlParser
    {
        private readonly HtmlDocument _document;

        public HtmlParser(string htmlContent)
        {
            _document = new HtmlDocument();
            _document.LoadHtml(htmlContent);
        }

        /// <summary>
        /// 指定されたタグから属性値を取得
        /// </summary>
        /// <param name="tagName">タグ名</param>
        /// <param name="attributeName">属性名</param>
        /// <returns>属性値のリスト</returns>
        public List<string> ExtractAttributeValues(string tagName, string attributeName)
        {
            return _document.DocumentNode
                .Descendants(tagName)
                .Where(node => node.GetAttributeValue(attributeName, null) != null)
                .Select(node => node.GetAttributeValue(attributeName, string.Empty))
                .ToList();
        }

        /// <summary>
        /// 指定されたタグのテキストコンテンツを取得
        /// </summary>
        /// <param name="tagName">タグ名</param>
        /// <returns>テキストコンテンツのリスト</returns>
        public List<string> ExtractTextContent(string tagName)
        {
            return _document.DocumentNode
                .Descendants(tagName)
                .Select(node => node.InnerText?.Trim())
                .Where(text => !string.IsNullOrEmpty(text))
                .ToList();
        }

        /// <summary>
        /// CSSセレクターを使用して要素を選択し、テキストを取得
        /// </summary>
        /// <param name="cssSelector">CSSセレクター</param>
        /// <returns>選択された要素のテキストコンテンツのリスト</returns>
        public List<string> ExtractByCssSelector(string cssSelector)
        {
            // HtmlAgilityPackではCSSセレクターの代わりにXPathを使用
            var xpath = ConvertCssSelectorToXPath(cssSelector);
            return ExtractByXPath(xpath);
        }

        /// <summary>
        /// 簡単なCSSセレクターをXPathに変換
        /// </summary>
        private string ConvertCssSelectorToXPath(string cssSelector)
        {
            // 基本的なCSSセレクターのみサポート
            if (cssSelector.StartsWith("."))
            {
                // クラスセレクター: .classname -> //*[@class='classname']
                var className = cssSelector.Substring(1);
                return $"//*[contains(@class, '{className}')]";
            }
            else if (cssSelector.StartsWith("#"))
            {
                // IDセレクター: #id -> //*[@id='id']
                var id = cssSelector.Substring(1);
                return $"//*[@id='{id}']";
            }
            else if (cssSelector.Contains("."))
            {
                // タグ+クラス: tag.classname -> //tag[contains(@class, 'classname')]
                var parts = cssSelector.Split('.');
                return $"//{parts[0]}[contains(@class, '{parts[1]}')]";
            }
            else
            {
                // 単純なタグセレクター: tag -> //tag
                return $"//{cssSelector}";
            }
        }

        /// <summary>
        /// XPathを使用して要素を選択し、テキストを取得
        /// </summary>
        /// <param name="xpath">XPath式</param>
        /// <returns>選択された要素のテキストコンテンツのリスト</returns>
        public List<string> ExtractByXPath(string xpath)
        {
            return _document.DocumentNode
                .SelectNodes(xpath)?
                .Select(node => node.InnerText?.Trim())
                .Where(text => !string.IsNullOrEmpty(text))
                .ToList() ?? new List<string>();
        }

        /// <summary>
        /// 指定されたタグとクラス名の組み合わせからデータを取得
        /// </summary>
        /// <param name="tagName">タグ名</param>
        /// <param name="className">クラス名</param>
        /// <returns>該当する要素のテキストコンテンツのリスト</returns>
        public List<string> ExtractByTagAndClass(string tagName, string className)
        {
            return _document.DocumentNode
                .Descendants(tagName)
                .Where(node => node.GetClasses().Contains(className))
                .Select(node => node.InnerText?.Trim())
                .Where(text => !string.IsNullOrEmpty(text))
                .ToList();
        }

        /// <summary>
        /// すべてのリンク（href属性）を取得
        /// </summary>
        /// <returns>リンクURLのリスト</returns>
        public List<string> ExtractAllLinks()
        {
            return ExtractAttributeValues("a", "href");
        }

        /// <summary>
        /// すべての画像（src属性）を取得
        /// </summary>
        /// <returns>画像URLのリスト</returns>
        public List<string> ExtractAllImages()
        {
            return ExtractAttributeValues("img", "src");
        }
    }
}