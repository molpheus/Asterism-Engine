# DOM解析システム (DOM Analysis System)

このシステムはHTML/XMLドキュメントのDOM構造を解析し、様々な分析を行うためのプラグイン対応フレームワークです。

This system is a plugin-ready framework for analyzing DOM structure of HTML/XML documents and performing various analyses.

## 機能 (Features)

### 🔍 DOM解析 (DOM Parsing)
- 正規表現ベースのHTMLパーサー
- タグ、属性、テキストコンテンツの抽出
- 階層構造の構築
- メタデータの自動抽出

### 📊 構造解析 (Structure Analysis)
- 見出し構造の解析と階層チェック
- リンク構造の分析（内部/外部リンク）
- 画像要素の解析（alt属性チェック等）
- セマンティック要素の使用状況分析
- コンテンツ密度の計算

### 🔌 プラグインアーキテクチャ (Plugin Architecture)
- `IDomAnalyzer`インターフェースによる拡張可能設計
- 独立した解析器の追加が容易
- 設定可能な解析パラメータ

## 使用方法 (Usage)

### 基本的なDOM解析 (Basic DOM Parsing)

```csharp
using Asterism.Common.System.DomAnalysis.Parser;
using Asterism.Common.System.DomAnalysis.Models;

// 解析設定を作成
var config = ParsingConfig.CreateDefault();

// パーサーを初期化
var parser = new DomParser(config);

// HTMLを解析
var document = parser.Parse(htmlContent);

// 結果の確認
Console.WriteLine($"タイトル: {document.Title}");
Console.WriteLine($"要素数: {document.AllElements.Count}");
Console.WriteLine($"最大深度: {document.Statistics.MaxDepth}");
```

### 構造解析の実行 (Structure Analysis)

```csharp
using Asterism.Common.System.DomAnalysis.Analyzers;

// 構造解析器を作成
var analyzer = new StructureAnalyzer();

// ドキュメントを解析
var result = analyzer.Analyze(document);

// 結果の確認
Console.WriteLine($"解析成功: {result.Success}");
Console.WriteLine($"信頼度: {result.Confidence:P}");

foreach (var data in result.Data)
{
    Console.WriteLine($"{data.Key}: {data.Value}");
}
```

### カスタム設定 (Custom Configuration)

```csharp
// 軽量設定（テキストのみ）
var lightConfig = ParsingConfig.CreateLightweight();

// フル機能設定
var fullConfig = ParsingConfig.CreateFullFeatured();

// カスタム設定
var customConfig = new ParsingConfig
{
    TargetTags = new List<string> { "h1", "h2", "p", "a" },
    ExtractText = true,
    ExtractAttributes = true,
    MaxElements = 50,
    IgnoreEmptyText = true
};
```

### 特定要素の検索 (Finding Specific Elements)

```csharp
// タグ名で検索
var headings = document.FindElementsByTag("h1");

// 属性で検索
var imagesWithAlt = document.FindElementsByAttribute("alt");

// CSS風セレクター（簡易版）
var mainContent = document.FindElementsBySelector("#main");
var navItems = document.FindElementsBySelector(".nav-item");
```

## 設定オプション (Configuration Options)

### ParsingConfig

| プロパティ | 説明 | デフォルト値 |
|-----------|------|------------|
| `TargetTags` | 解析対象タグリスト | `["h1", "h2", "h3", "h4", "h5", "h6", "p", "a", "img", "div", "span"]` |
| `ExtractText` | テキスト抽出の有効化 | `true` |
| `ExtractAttributes` | 属性抽出の有効化 | `true` |
| `ExtractInnerHtml` | 内部HTML抽出の有効化 | `false` |
| `MaxElements` | 最大解析要素数 | `0` (無制限) |
| `IgnoreEmptyText` | 空テキストの無視 | `true` |
| `DecodeHtmlEntities` | HTMLエンティティのデコード | `true` |
| `NormalizeWhitespace` | 空白の正規化 | `true` |

## プラグイン開発 (Plugin Development)

新しい解析器を作成するには、`IDomAnalyzer`インターフェースを実装します：

```csharp
public class CustomAnalyzer : IDomAnalyzer
{
    public string Name => "Custom Analyzer";
    public string Version => "1.0.0";
    public string Description => "カスタム解析器の説明";

    public bool CanAnalyze(DomDocument document)
    {
        // 解析可能かどうかの判定ロジック
        return document != null && document.AllElements.Count > 0;
    }

    public AnalysisResult Analyze(DomDocument document)
    {
        var result = new AnalysisResult { AnalyzerName = Name };
        
        // 解析ロジックを実装
        // 結果をresult.AddData()で追加
        
        return result;
    }
}
```

## アーキテクチャ (Architecture)

```
DomAnalysis/
├── Models/              # データモデル
│   ├── DomElement.cs    # DOM要素表現
│   ├── DomDocument.cs   # DOM文書表現
│   └── ParsingConfig.cs # 解析設定
├── Parser/              # 解析エンジン
│   └── DomParser.cs     # DOM解析器
└── Analyzers/           # 分析プラグイン
    ├── IDomAnalyzer.cs     # 解析器インターフェース
    └── StructureAnalyzer.cs # 構造解析器
```

## 技術的特徴 (Technical Features)

- **外部依存なし**: 正規表現ベースの解析で外部ライブラリ不要
- **高性能**: 効率的な解析アルゴリズム
- **拡張可能**: プラグインアーキテクチャによる機能拡張
- **設定可能**: 柔軟な解析設定オプション
- **エラーハンドリング**: 包括的なエラー処理と警告システム

## 将来の拡張 (Future Extensions)

- XPath風のクエリサポート
- CSSセレクターの完全サポート
- JavaScript実行結果の解析
- より高度なセマンティック解析
- パフォーマンス最適化オプション

## 使用例 (Examples)

詳細な使用例は、各クラスのドキュメントコメントを参照してください。また、実際のHTMLドキュメントでのテストケースも含まれています。