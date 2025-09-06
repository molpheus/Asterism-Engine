using System;
using System.Collections.Generic;
using System.Linq;
using Asterism.Common.System.DomAnalysis.Models;

namespace Asterism.Common.System.DomAnalysis.Analyzers
{
    /// <summary>
    /// DOM構造解析器 - 文書の構造を解析
    /// DOM structure analyzer - analyzes document structure
    /// </summary>
    public class StructureAnalyzer : IDomAnalyzer
    {
        public string Name => "Structure Analyzer";
        public string Version => "1.0.0";
        public string Description => "DOMドキュメントの構造的特徴を解析します / Analyzes structural characteristics of DOM documents";

        /// <summary>
        /// DOMドキュメントの構造を解析
        /// Analyze DOM document structure
        /// </summary>
        public AnalysisResult Analyze(DomDocument document)
        {
            var result = new AnalysisResult
            {
                AnalyzerName = Name
            };

            try
            {
                // 基本統計
                // Basic statistics
                AnalyzeBasicStatistics(document, result);

                // 見出し構造の解析
                // Analyze heading structure
                AnalyzeHeadingStructure(document, result);

                // リンク解析
                // Analyze links
                AnalyzeLinkStructure(document, result);

                // 画像解析
                // Analyze images
                AnalyzeImageStructure(document, result);

                // コンテンツ密度解析
                // Analyze content density
                AnalyzeContentDensity(document, result);

                // セマンティック要素解析
                // Analyze semantic elements
                AnalyzeSemanticElements(document, result);

                result.Confidence = CalculateConfidence(document, result);
            }
            catch (Exception ex)
            {
                result.AddError($"構造解析中にエラーが発生しました: {ex.Message}");
                result.Confidence = 0.0;
            }

            return result;
        }

        /// <summary>
        /// この解析器がドキュメントを解析可能かチェック
        /// Check if this analyzer can analyze the document
        /// </summary>
        public bool CanAnalyze(DomDocument document)
        {
            return document != null && document.AllElements != null && document.AllElements.Count > 0;
        }

        /// <summary>
        /// 基本統計を解析
        /// Analyze basic statistics
        /// </summary>
        private void AnalyzeBasicStatistics(DomDocument document, AnalysisResult result)
        {
            var stats = document.Statistics;
            
            result.AddData("総要素数", stats.TotalElements);
            result.AddData("最大深度", stats.MaxDepth);
            result.AddData("タグ種類数", stats.TagCounts.Count);
            result.AddData("最も多いタグ", stats.TagCounts.OrderByDescending(x => x.Value).FirstOrDefault().Key ?? "N/A");
            
            // タグ分布
            var tagDistribution = stats.TagCounts.OrderByDescending(x => x.Value)
                                               .Take(10)
                                               .ToDictionary(x => x.Key, x => x.Value);
            result.AddData("タグ分布トップ10", tagDistribution);
        }

        /// <summary>
        /// 見出し構造を解析
        /// Analyze heading structure
        /// </summary>
        private void AnalyzeHeadingStructure(DomDocument document, AnalysisResult result)
        {
            var headings = document.AllElements
                .Where(e => new[] { "h1", "h2", "h3", "h4", "h5", "h6" }.Contains(e.TagName))
                .OrderBy(e => e.Position)
                .ToList();

            var headingStructure = new Dictionary<string, object>
            {
                ["総見出し数"] = headings.Count,
                ["H1数"] = headings.Count(h => h.TagName == "h1"),
                ["H2数"] = headings.Count(h => h.TagName == "h2"),
                ["H3数"] = headings.Count(h => h.TagName == "h3"),
                ["H4数"] = headings.Count(h => h.TagName == "h4"),
                ["H5数"] = headings.Count(h => h.TagName == "h5"),
                ["H6数"] = headings.Count(h => h.TagName == "h6")
            };

            // 見出しの階層構造チェック
            var hierarchyIssues = CheckHeadingHierarchy(headings);
            if (hierarchyIssues.Count > 0)
            {
                headingStructure["階層問題"] = hierarchyIssues;
                result.AddWarning($"見出しの階層に{hierarchyIssues.Count}個の問題があります");
            }

            result.AddData("見出し構造", headingStructure);
        }

        /// <summary>
        /// リンク構造を解析
        /// Analyze link structure
        /// </summary>
        private void AnalyzeLinkStructure(DomDocument document, AnalysisResult result)
        {
            var links = document.AllElements.Where(e => e.TagName == "a").ToList();
            
            var internalLinks = links.Where(l => IsInternalLink(l.GetAttribute("href"))).ToList();
            var externalLinks = links.Where(l => IsExternalLink(l.GetAttribute("href"))).ToList();
            var emptyLinks = links.Where(l => string.IsNullOrWhiteSpace(l.GetAttribute("href"))).ToList();

            var linkStructure = new Dictionary<string, object>
            {
                ["総リンク数"] = links.Count,
                ["内部リンク数"] = internalLinks.Count,
                ["外部リンク数"] = externalLinks.Count,
                ["空リンク数"] = emptyLinks.Count,
                ["テキストなしリンク数"] = links.Count(l => string.IsNullOrWhiteSpace(l.InnerText))
            };

            if (emptyLinks.Count > 0)
            {
                result.AddWarning($"{emptyLinks.Count}個の空のリンクが見つかりました");
            }

            result.AddData("リンク構造", linkStructure);
        }

        /// <summary>
        /// 画像構造を解析
        /// Analyze image structure
        /// </summary>
        private void AnalyzeImageStructure(DomDocument document, AnalysisResult result)
        {
            var images = document.AllElements.Where(e => e.TagName == "img").ToList();
            
            var imagesWithoutAlt = images.Where(img => string.IsNullOrWhiteSpace(img.GetAttribute("alt"))).ToList();
            var imagesWithoutSrc = images.Where(img => string.IsNullOrWhiteSpace(img.GetAttribute("src"))).ToList();

            var imageStructure = new Dictionary<string, object>
            {
                ["総画像数"] = images.Count,
                ["alt属性なし画像数"] = imagesWithoutAlt.Count,
                ["src属性なし画像数"] = imagesWithoutSrc.Count
            };

            if (imagesWithoutAlt.Count > 0)
            {
                result.AddWarning($"{imagesWithoutAlt.Count}個の画像にalt属性がありません（アクセシビリティの問題）");
            }

            if (imagesWithoutSrc.Count > 0)
            {
                result.AddError($"{imagesWithoutSrc.Count}個の画像にsrc属性がありません");
            }

            result.AddData("画像構造", imageStructure);
        }

        /// <summary>
        /// コンテンツ密度を解析
        /// Analyze content density
        /// </summary>
        private void AnalyzeContentDensity(DomDocument document, AnalysisResult result)
        {
            var textElements = document.AllElements.Where(e => !string.IsNullOrWhiteSpace(e.InnerText)).ToList();
            var totalTextLength = textElements.Sum(e => e.InnerText?.Length ?? 0);
            var averageTextLength = textElements.Count > 0 ? totalTextLength / (double)textElements.Count : 0;

            var contentDensity = new Dictionary<string, object>
            {
                ["テキスト要素数"] = textElements.Count,
                ["総テキスト長"] = totalTextLength,
                ["平均テキスト長"] = Math.Round(averageTextLength, 2),
                ["テキスト密度"] = Math.Round((double)textElements.Count / document.AllElements.Count * 100, 2)
            };

            result.AddData("コンテンツ密度", contentDensity);
        }

        /// <summary>
        /// セマンティック要素を解析
        /// Analyze semantic elements
        /// </summary>
        private void AnalyzeSemanticElements(DomDocument document, AnalysisResult result)
        {
            var semanticTags = new[] { "header", "nav", "main", "section", "article", "aside", "footer" };
            var semanticElements = document.AllElements
                .Where(e => semanticTags.Contains(e.TagName))
                .GroupBy(e => e.TagName)
                .ToDictionary(g => g.Key, g => g.Count());

            var semanticStructure = new Dictionary<string, object>
            {
                ["セマンティック要素総数"] = semanticElements.Values.Sum(),
                ["使用されているセマンティックタグ"] = semanticElements.Keys.ToList(),
                ["セマンティック要素分布"] = semanticElements
            };

            // HTML5セマンティック要素の使用率
            var semanticUsageRate = (double)semanticElements.Count / semanticTags.Length * 100;
            semanticStructure["セマンティック使用率"] = Math.Round(semanticUsageRate, 2);

            if (semanticUsageRate < 30)
            {
                result.AddWarning("セマンティック要素の使用率が低いです。HTML5のセマンティック要素の使用を検討してください");
            }

            result.AddData("セマンティック構造", semanticStructure);
        }

        /// <summary>
        /// 見出しの階層構造をチェック
        /// Check heading hierarchy structure
        /// </summary>
        private List<string> CheckHeadingHierarchy(List<DomElement> headings)
        {
            var issues = new List<string>();
            var previousLevel = 0;

            foreach (var heading in headings)
            {
                var currentLevel = int.Parse(heading.TagName.Substring(1)); // h1 -> 1, h2 -> 2, etc.
                
                if (previousLevel > 0 && currentLevel > previousLevel + 1)
                {
                    issues.Add($"見出しレベルがスキップされています: H{previousLevel} の後に H{currentLevel}");
                }
                
                previousLevel = currentLevel;
            }

            return issues;
        }

        /// <summary>
        /// 内部リンクかどうかを判定
        /// Determine if link is internal
        /// </summary>
        private bool IsInternalLink(string href)
        {
            if (string.IsNullOrWhiteSpace(href))
                return false;
                
            return href.StartsWith("#") || href.StartsWith("/") || 
                   (!href.StartsWith("http://") && !href.StartsWith("https://") && !href.StartsWith("mailto:"));
        }

        /// <summary>
        /// 外部リンクかどうかを判定
        /// Determine if link is external
        /// </summary>
        private bool IsExternalLink(string href)
        {
            if (string.IsNullOrWhiteSpace(href))
                return false;
                
            return href.StartsWith("http://") || href.StartsWith("https://");
        }

        /// <summary>
        /// 信頼度スコアを計算
        /// Calculate confidence score
        /// </summary>
        private double CalculateConfidence(DomDocument document, AnalysisResult result)
        {
            double confidence = 1.0;
            
            // エラーがある場合は信頼度を下げる
            // Reduce confidence if there are errors
            if (result.Errors.Count > 0)
                confidence -= 0.3;
                
            // 警告がある場合は軽く信頼度を下げる
            // Slightly reduce confidence if there are warnings
            if (result.Warnings.Count > 0)
                confidence -= 0.1 * Math.Min(result.Warnings.Count, 3) / 3.0;
                
            // 要素数が少ない場合は信頼度を下げる
            // Reduce confidence if element count is low
            if (document.AllElements.Count < 10)
                confidence -= 0.2;

            return Math.Max(0.0, confidence);
        }
    }
}