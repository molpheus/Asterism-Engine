# Webコンテンツクローニング機能

## 概要
Asterism EngineのWebコンテンツクローニング機能は、HTMLページから特定のタグやデータを解析し、抽出するためのC#基礎コードです。

## 主な機能

### 1. WebContentCloner
メインクラスで、Webコンテンツの取得とHTML解析を統合します。

- **基本的なクローニング**: URLからHTMLコンテンツを取得し、解析します
- **特定タグの抽出**: 指定されたHTMLタグのみを抽出します
- **カスタム設定**: 抽出する内容を細かく設定できます

### 2. HtmlParser
シンプルなHTML解析エンジンで、以下の機能を提供します：

- **タグ解析**: HTMLタグとその属性を抽出
- **テキスト抽出**: HTMLタグを除去してテキストのみを取得
- **メタデータ抽出**: メタタグからページ情報を取得

### 3. データモデル

#### WebContent
- URL、タイトル、取得日時
- 生のHTMLコンテンツ
- 抽出された要素のリスト
- メタデータ辞書

#### ExtractedElement
- タグ名、テキスト、HTML
- 属性辞書
- 位置情報

#### ExtractionConfig
- 対象タグの指定
- 抽出オプションの設定
- 最大要素数の制限

## 使用例

### 基本的な使用方法
```csharp
using (var cloner = new WebContentCloner())
{
    var content = await cloner.CloneAsync("https://example.com");
    Console.WriteLine($"Title: {content.Title}");
    Console.WriteLine($"Elements: {content.Elements.Count}");
}
```

### 特定タグの抽出
```csharp
using (var cloner = new WebContentCloner())
{
    var content = await cloner.CloneSpecificTagsAsync("https://example.com", 
        new[] { "h1", "h2", "a" });
    
    foreach (var element in content.Elements)
    {
        Console.WriteLine($"{element.TagName}: {element.InnerText}");
    }
}
```

### カスタム設定
```csharp
var config = new ExtractionConfig
{
    ExtractText = true,
    ExtractAttributes = true,
    MaxElements = 50
};
config.TargetTags.Add("title");
config.TargetTags.Add("p");

using (var cloner = new WebContentCloner(config))
{
    var content = await cloner.CloneAsync("https://example.com");
}
```

## アーキテクチャ

```
WebContentCloner
├── WebConnecter (HTTP通信)
├── HtmlParser (HTML解析)
└── Models
    ├── WebContent (結果データ)
    ├── ExtractedElement (要素データ)
    └── ExtractionConfig (設定)
```

## 制限事項

- .NET Framework 4.8をターゲット
- 外部HTMLパーサーライブラリは使用せず、正規表現ベースの解析
- JavaScript実行には対応していません
- 複雑なCSSセレクターには対応していません

## 今後の拡張案

1. **HtmlAgilityPack統合**: より高度なHTML解析
2. **CSS セレクター対応**: より柔軟な要素選択
3. **非同期バッチ処理**: 複数URLの並列処理
4. **キャッシュ機能**: 重複アクセスの回避
5. **ロボットアクセスルール対応**: robots.txt の尊重

## エラーハンドリング

```csharp
try
{
    var content = await cloner.CloneAsync("https://example.com");
}
catch (WebCloningException ex)
{
    Console.WriteLine($"クローニング失敗: {ex.Message}");
}
```

## パフォーマンス

- 接続プール使用によるHTTP接続の再利用
- 最大要素数制限による大きなページでのメモリ使用量制御
- CancellationToken対応による中断可能操作