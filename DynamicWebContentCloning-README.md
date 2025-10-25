# 動的Webコンテンツクローニング機能

このドキュメントでは、JavaScript動的コンテンツを含むWebサイトからの情報抽出機能について説明します。

## 概要

従来の静的HTMLパーサーでは、JavaScriptによって動的に生成されるコンテンツを取得することができませんでした。本機能では、Selenium WebDriverを使用してブラウザを自動制御し、JavaScriptが実行された後の完全にレンダリングされたコンテンツを抽出することができます。

## 主要機能

### 1. 動的コンテンツ抽出器 (`DynamicContentExtractor`)

- **ヘッドレスブラウザ制御**: Chrome/Chromiumを使用した自動ブラウザ操作
- **JavaScript実行待機**: ページの完全な読み込み完了まで待機
- **要素待機**: 特定の要素が表示されるまで待機
- **無限スクロール対応**: 自動スクロールによる追加コンテンツ取得
- **カスタムスクリプト実行**: 独自のJavaScriptを実行してページを操作

### 2. 拡張されたWebContentCloner

既存の静的コンテンツ抽出機能に加えて、以下の動的機能を提供：

- `CloneDynamicAsync()` - 基本的な動的コンテンツ抽出
- `CloneDynamicWithElementWaitAsync()` - 要素待機型抽出
- `CloneInfiniteScrollAsync()` - 無限スクロール対応抽出
- `CloneDynamicWithScriptAsync()` - カスタムスクリプト実行型抽出

## 使用方法

### 基本的な動的コンテンツ抽出

```csharp
// 動的抽出オプションの設定
var options = new DynamicExtractionOptions
{
    Headless = true,
    PageLoadTimeoutSeconds = 30,
    AdditionalWaitSeconds = 3.0
};

var cloner = new WebContentCloner(options);

// JavaScriptで動的生成されるサイトから抽出
var result = await cloner.CloneDynamicAsync("https://quotes.toscrape.js.org/");

if (result.Success)
{
    Console.WriteLine($"タイトル: {result.Content.Metadata.Title}");
    Console.WriteLine($"見出し数: {result.Content.Headings.Count}");
    Console.WriteLine($"動的コンテンツ: {result.IsDynamic}");
}
```

### 特定要素の表示待機

```csharp
// 特定の要素（.quote）が表示されるまで待機
var result = await cloner.CloneDynamicWithElementWaitAsync(
    "https://quotes.toscrape.js.org/", 
    ".quote");

if (result.Success)
{
    var quotes = result.Content.Paragraphs;
    Console.WriteLine($"取得した引用: {quotes.Count}件");
}
```

### 無限スクロールサイトの対応

```csharp
// 最大3回スクロールして追加コンテンツを取得
var result = await cloner.CloneInfiniteScrollAsync(
    "https://quotes.toscrape.js.org/scroll", 
    maxScrolls: 3);

if (result.Success)
{
    Console.WriteLine($"全段落数: {result.Content.Paragraphs.Count}");
}
```

### カスタムJavaScript実行

```csharp
var customScript = @"
    // ページ内のボタンをクリック
    var button = document.querySelector('.load-more');
    if (button) {
        button.click();
    }
";

var result = await cloner.CloneDynamicWithScriptAsync(
    "https://example.com", 
    customScript);
```

## 設定オプション

### DynamicExtractionOptions

| プロパティ | 型 | デフォルト値 | 説明 |
|------------|-----|------------|------|
| `Headless` | bool | true | ヘッドレスモードで実行するか |
| `PageLoadTimeoutSeconds` | int | 30 | ページロードのタイムアウト時間（秒） |
| `AdditionalWaitSeconds` | double | 2.0 | 追加の待機時間（秒） |
| `UserAgent` | string | "" | カスタムユーザーエージェント |

```csharp
var options = new DynamicExtractionOptions
{
    Headless = false,              // ブラウザを表示
    PageLoadTimeoutSeconds = 60,   // 60秒でタイムアウト
    AdditionalWaitSeconds = 5.0,   // 5秒追加待機
    UserAgent = "MyBot/1.0"        // カスタムUA
};
```

## パフォーマンス特性

### 静的 vs 動的抽出の比較

| 抽出方式 | 処理時間 | メモリ使用量 | JavaScript対応 |
|----------|----------|-------------|--------------|
| 静的抽出 | 0.1-1秒 | 低い | ❌ |
| 動的抽出 | 2-10秒 | 高い | ✅ |

### 推奨用途

**静的抽出を使用**:
- サーバーサイドレンダリング（SSR）サイト
- 従来のHTMLサイト
- 高速処理が必要な場合

**動的抽出を使用**:
- Single Page Application（SPA）
- JavaScript依存のコンテンツ
- 無限スクロールサイト
- 動的ローディングコンテンツ

## エラーハンドリング

```csharp
try
{
    var result = await cloner.CloneDynamicAsync(url);
    
    if (!result.Success)
    {
        Console.WriteLine($"エラー: {result.ErrorMessage}");
        return;
    }
    
    // 成功時の処理
}
catch (WebDriverException ex)
{
    Console.WriteLine($"ブラウザエラー: {ex.Message}");
}
catch (TimeoutException ex)
{
    Console.WriteLine($"タイムアウト: {ex.Message}");
}
```

## システム要件

### 必要な依存関係

- **.NET 8.0以上**
- **Selenium.WebDriver 4.16.2以上**
- **Selenium.WebDriver.ChromeDriver 120以上**
- **Chrome/Chromium ブラウザ**（システムにインストール済み）

### 環境設定

#### Windowsの場合
ChromeDriverは自動的にインストールされます。

#### Linuxの場合
```bash
# Chromeのインストール
sudo apt-get update
sudo apt-get install -y google-chrome-stable

# または Chromium
sudo apt-get install -y chromium-browser
```

#### Dockerの場合
```dockerfile
# Dockerfile例
FROM mcr.microsoft.com/dotnet/aspnet:8.0
RUN apt-get update && apt-get install -y \
    google-chrome-stable \
    && rm -rf /var/lib/apt/lists/*
```

## 使用上の注意点

### 法的・倫理的考慮事項

1. **robots.txt の確認**: サイトのrobots.txtを確認し、クローリング制限を遵守
2. **利用規約の確認**: 対象サイトの利用規約を確認
3. **リクエスト頻度の制限**: 過度なリクエストを避け、適切な間隔を設ける
4. **著作権の尊重**: 取得したコンテンツの利用は適切な権利範囲内で

### パフォーマンス最適化

1. **ヘッドレスモード使用**: `Headless = true` で高速化
2. **適切なタイムアウト設定**: 長すぎず短すぎない適切な値を設定
3. **リソース管理**: `using` ステートメントでDynamicContentExtractorを適切に破棄
4. **並列処理の制限**: 同時実行するブラウザインスタンス数を制限

## トラブルシューティング

### よくある問題と解決策

#### ChromeDriverが見つからない
```
WebDriverException: chromedriver not found
```
**解決策**: Selenium.WebDriver.ChromeDriverパッケージが正しくインストールされているか確認

#### タイムアウトエラー
```
TimeoutException: Timeout occurred
```
**解決策**: `PageLoadTimeoutSeconds` と `AdditionalWaitSeconds` を増加

#### 要素が見つからない
```
NoSuchElementException: Unable to locate element
```
**解決策**: 要素セレクターを確認し、要素の表示に十分な待機時間を設定

## サンプルコード

完全な使用例は `DynamicWebContentExample.cs` クラスを参照してください。以下の例が含まれています：

- 基本的な動的コンテンツ抽出
- 要素待機による抽出
- 無限スクロール対応
- カスタムスクリプト実行
- 静的vs動的抽出の比較

```csharp
// すべての例を実行
await DynamicWebContentExample.RunAllExamples();
```

## 今後の拡張予定

- **Firefox/Edge対応**: Chrome以外のブラウザサポート
- **プロキシ対応**: プロキシサーバー経由でのアクセス
- **認証対応**: ログインが必要なサイトへの対応
- **モバイルエミュレーション**: モバイル端末のエミュレーション
- **スクリーンショット機能**: ページのスクリーンショット取得