# Web Content Cloning Foundation - Webコンテンツクローニング基盤

このプロジェクトは、C#でWebコンテンツのタグ解析を行い、特定のデータを取得するクローニングの基礎機能を提供します。

## 概要

Web Content Cloning Foundationは以下の機能を提供します：

- **HTMLパーサー**: HTMLコンテンツからタグや属性値、テキストコンテンツを抽出
- **Webコンテンツクローナー**: Webサイトからコンテンツを取得し、構造化データとして解析
- **データモデル**: 解析されたWebコンテンツを表現するためのクラス群
- **接続管理**: 効率的なHTTP接続プールによるWebアクセス

## 主要クラス

### HtmlParser
HTMLコンテンツの解析と特定データの抽出を行うクラス

```csharp
var parser = new HtmlParser(htmlContent);

// タグのテキストコンテンツを取得
var titles = parser.ExtractTextContent("title");

// 属性値を取得
var links = parser.ExtractAttributeValues("a", "href");

// XPathを使用した抽出
var metaData = parser.ExtractByXPath("//meta[@name='description']");

// CSSセレクターを使用した抽出（基本的なもののみ）
var headings = parser.ExtractByCssSelector("h1");
```

### WebContentCloner
Webコンテンツのクローニングと解析を行うメインクラス

```csharp
var cloner = new WebContentCloner();

// Webサイト全体を解析
var result = await cloner.CloneAsync("https://example.com");

if (result.Success)
{
    Console.WriteLine($"タイトル: {result.Content.Metadata.Title}");
    Console.WriteLine($"リンク数: {result.Content.Links.Count}");
}

// 特定のデータのみを抽出
var links = await cloner.ExtractAttributeValuesAsync("https://example.com", "a", "href");
```

### データモデル

- **ParsedWebContent**: 解析されたWebコンテンツの完全な情報
- **WebPageMetadata**: ページのメタデータ（タイトル、説明など）
- **ExtractedTagData**: 特定のHTMLタグから抽出されたデータ
- **WebContentCloneResult**: クローニング操作の結果

## 使用例

### 基本的な使用方法

```csharp
using Asterism.Common.Network;

// HTMLパーサーの直接使用
var htmlContent = "<html><title>テスト</title><body><h1>見出し</h1></body></html>";
var parser = new HtmlParser(htmlContent);

var titles = parser.ExtractTextContent("title");
var headings = parser.ExtractTextContent("h1");

Console.WriteLine($"タイトル: {titles[0]}");
Console.WriteLine($"見出し: {headings[0]}");
```

### Webサイトからのデータ取得

```csharp
var cloner = new WebContentCloner();
var result = await cloner.CloneAsync("https://example.com");

if (result.Success)
{
    var content = result.Content;
    
    Console.WriteLine($"URL: {content.Url}");
    Console.WriteLine($"タイトル: {content.Metadata.Title}");
    Console.WriteLine($"説明: {content.Metadata.Description}");
    Console.WriteLine($"処理時間: {result.ProcessingTime.TotalMilliseconds}ms");
    
    // 取得したリンクを表示
    foreach (var link in content.Links)
    {
        Console.WriteLine($"リンク: {link}");
    }
}
```

### カスタムデータ抽出

```csharp
// 特定のCSSセレクターに一致する要素を抽出
var headings = await cloner.ExtractByCssSelectorAsync("https://example.com", "h1");

// 特定の属性値を抽出
var imageUrls = await cloner.ExtractAttributeValuesAsync("https://example.com", "img", "src");
```

## 特徴

### HTMLパーサーの機能

- **タグベースの抽出**: 指定したタグのテキストコンテンツを取得
- **属性値の抽出**: 任意のタグの属性値を取得
- **XPath対応**: XPath式を使用した高度な要素選択
- **基本的なCSSセレクター**: クラスやIDによる要素選択
- **便利メソッド**: リンクや画像の一括取得

### Webコネクターの機能

- **接続プール**: 効率的なHTTP接続の再利用
- **非同期処理**: 高速なWebコンテンツ取得
- **エラーハンドリング**: 適切な例外処理とエラー報告

### データモデルの特徴

- **構造化**: 解析されたデータの体系的な管理
- **拡張性**: 新しいデータタイプの追加が容易
- **型安全**: 強い型付けによる安全性

## 技術仕様

- **.NET 8.0** 対応
- **HtmlAgilityPack** を使用したHTML解析
- **非同期プログラミング** パターンの採用
- **ユニットテスト** による品質保証

## テスト

プロジェクトには包括的なユニットテストが含まれています：

```bash
dotnet test
```

現在、37個のテストがすべて正常に実行されています。

## パフォーマンス

- **接続プール**: 最大10個のHTTP接続を再利用
- **メモリ効率**: 必要最小限のメモリ使用量
- **高速処理**: 非同期処理による高いスループット

## 注意事項

- **robots.txt**: Webサイトのクローリング前にrobots.txtを確認してください
- **レート制限**: 対象サイトに負荷をかけないよう適切な間隔を設けてください
- **法的遵守**: 著作権やサイトの利用規約を遵守してください

## ライセンス

このプロジェクトはAsterism Engineプロジェクトの一部として提供されています。