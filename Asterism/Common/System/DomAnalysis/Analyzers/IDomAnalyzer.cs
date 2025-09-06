using System.Collections.Generic;
using Asterism.Common.System.DomAnalysis.Models;

namespace Asterism.Common.System.DomAnalysis.Analyzers
{
    /// <summary>
    /// DOM解析器のインターフェース - プラグイン対応
    /// Interface for DOM analyzers - plugin ready
    /// </summary>
    public interface IDomAnalyzer
    {
        /// <summary>
        /// 解析器の名前
        /// Name of the analyzer
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 解析器のバージョン
        /// Version of the analyzer
        /// </summary>
        string Version { get; }

        /// <summary>
        /// 解析器の説明
        /// Description of the analyzer
        /// </summary>
        string Description { get; }

        /// <summary>
        /// DOMドキュメントを解析して結果を返す
        /// Analyze DOM document and return results
        /// </summary>
        /// <param name="document">解析対象のDOMドキュメント</param>
        /// <returns>解析結果</returns>
        AnalysisResult Analyze(DomDocument document);

        /// <summary>
        /// この解析器が指定されたドキュメントを解析可能かどうか
        /// Whether this analyzer can analyze the specified document
        /// </summary>
        /// <param name="document">チェック対象のドキュメント</param>
        /// <returns>解析可能な場合はtrue</returns>
        bool CanAnalyze(DomDocument document);
    }

    /// <summary>
    /// DOM解析結果
    /// DOM analysis result
    /// </summary>
    public class AnalysisResult
    {
        /// <summary>
        /// 解析器名
        /// Analyzer name
        /// </summary>
        public string AnalyzerName { get; set; }

        /// <summary>
        /// 解析結果のデータ
        /// Analysis result data
        /// </summary>
        public Dictionary<string, object> Data { get; set; }

        /// <summary>
        /// 解析中に発生した警告
        /// Warnings occurred during analysis
        /// </summary>
        public List<string> Warnings { get; set; }

        /// <summary>
        /// 解析中に発生したエラー
        /// Errors occurred during analysis
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// 解析が成功したかどうか
        /// Whether the analysis was successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 信頼度スコア（0.0-1.0）
        /// Confidence score (0.0-1.0)
        /// </summary>
        public double Confidence { get; set; }

        public AnalysisResult()
        {
            Data = new Dictionary<string, object>();
            Warnings = new List<string>();
            Errors = new List<string>();
            Success = true;
            Confidence = 1.0;
        }

        /// <summary>
        /// 解析結果にデータを追加
        /// Add data to analysis result
        /// </summary>
        public void AddData(string key, object value)
        {
            Data[key] = value;
        }

        /// <summary>
        /// 警告を追加
        /// Add warning
        /// </summary>
        public void AddWarning(string warning)
        {
            Warnings.Add(warning);
        }

        /// <summary>
        /// エラーを追加
        /// Add error
        /// </summary>
        public void AddError(string error)
        {
            Errors.Add(error);
            Success = false;
        }
    }
}