using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Asterism.Common.Network
{
    /// <summary>
    /// JavaScript動的コンテンツの抽出を行うクラス
    /// </summary>
    public class DynamicContentExtractor : IDisposable
    {
        private readonly ChromeDriver _driver;
        private readonly WebDriverWait _wait;
        private readonly DynamicExtractionOptions _options;
        private bool _disposed = false;

        public DynamicContentExtractor() : this(new DynamicExtractionOptions())
        {
        }

        public DynamicContentExtractor(DynamicExtractionOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            
            var chromeOptions = new ChromeOptions();
            
            // ヘッドレスモードの設定
            if (_options.Headless)
            {
                chromeOptions.AddArgument("--headless");
            }
            
            // その他のChrome設定
            chromeOptions.AddArgument("--no-sandbox");
            chromeOptions.AddArgument("--disable-dev-shm-usage");
            chromeOptions.AddArgument("--disable-gpu");
            chromeOptions.AddArgument("--window-size=1920,1080");
            
            // ユーザーエージェントの設定
            if (!string.IsNullOrEmpty(_options.UserAgent))
            {
                chromeOptions.AddArgument($"--user-agent={_options.UserAgent}");
            }

            _driver = new ChromeDriver(chromeOptions);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(_options.PageLoadTimeoutSeconds));
        }

        /// <summary>
        /// 動的コンテンツを含むWebページからHTMLを取得
        /// </summary>
        /// <param name="url">対象URL</param>
        /// <param name="cancellationToken">キャンセレーショントークン</param>
        /// <returns>レンダリング後のHTML</returns>
        public async Task<string> GetRenderedHtmlAsync(string url, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                _driver.Navigate().GoToUrl(url);
                
                // 基本的なページロード完了を待つ
                _wait.Until(driver => ((IJavaScriptExecutor)driver)
                    .ExecuteScript("return document.readyState").Equals("complete"));
                
                // 追加の待機時間（動的コンテンツの読み込み用）
                if (_options.AdditionalWaitSeconds > 0)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(_options.AdditionalWaitSeconds));
                }
                
                cancellationToken.ThrowIfCancellationRequested();
                
                return _driver.PageSource;
            }, cancellationToken);
        }

        /// <summary>
        /// 特定の要素が表示されるまで待機してからHTMLを取得
        /// </summary>
        /// <param name="url">対象URL</param>
        /// <param name="elementSelector">待機する要素のCSSセレクター</param>
        /// <param name="cancellationToken">キャンセレーショントークン</param>
        /// <returns>レンダリング後のHTML</returns>
        public async Task<string> GetRenderedHtmlWithElementWaitAsync(string url, string elementSelector, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                _driver.Navigate().GoToUrl(url);
                
                try
                {
                    // 指定された要素が表示されるまで待機
                    _wait.Until(driver => driver.FindElement(By.CssSelector(elementSelector)).Displayed);
                }
                catch (WebDriverTimeoutException)
                {
                    // タイムアウトした場合でも現在のHTMLを返す
                }
                
                // 追加の待機時間
                if (_options.AdditionalWaitSeconds > 0)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(_options.AdditionalWaitSeconds));
                }
                
                cancellationToken.ThrowIfCancellationRequested();
                
                return _driver.PageSource;
            }, cancellationToken);
        }

        /// <summary>
        /// JavaScriptを実行してからHTMLを取得
        /// </summary>
        /// <param name="url">対象URL</param>
        /// <param name="script">実行するJavaScript</param>
        /// <param name="cancellationToken">キャンセレーショントークン</param>
        /// <returns>レンダリング後のHTML</returns>
        public async Task<string> GetRenderedHtmlWithScriptAsync(string url, string script, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                _driver.Navigate().GoToUrl(url);
                
                // ページロード完了を待つ
                _wait.Until(driver => ((IJavaScriptExecutor)driver)
                    .ExecuteScript("return document.readyState").Equals("complete"));
                
                // カスタムJavaScriptを実行
                if (!string.IsNullOrEmpty(script))
                {
                    ((IJavaScriptExecutor)_driver).ExecuteScript(script);
                }
                
                // 追加の待機時間
                if (_options.AdditionalWaitSeconds > 0)
                {
                    Thread.Sleep(TimeSpan.FromSeconds(_options.AdditionalWaitSeconds));
                }
                
                cancellationToken.ThrowIfCancellationRequested();
                
                return _driver.PageSource;
            }, cancellationToken);
        }

        /// <summary>
        /// 無限スクロールページの全コンテンツを取得
        /// </summary>
        /// <param name="url">対象URL</param>
        /// <param name="maxScrolls">最大スクロール回数</param>
        /// <param name="cancellationToken">キャンセレーショントークン</param>
        /// <returns>レンダリング後のHTML</returns>
        public async Task<string> GetInfiniteScrollContentAsync(string url, int maxScrolls = 10, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                
                _driver.Navigate().GoToUrl(url);
                
                // ページロード完了を待つ
                _wait.Until(driver => ((IJavaScriptExecutor)driver)
                    .ExecuteScript("return document.readyState").Equals("complete"));
                
                var lastHeight = (long)((IJavaScriptExecutor)_driver)
                    .ExecuteScript("return document.body.scrollHeight");
                
                for (int i = 0; i < maxScrolls; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    
                    // ページの最下部までスクロール
                    ((IJavaScriptExecutor)_driver)
                        .ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                    
                    // 新しいコンテンツの読み込みを待つ
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    
                    var newHeight = (long)((IJavaScriptExecutor)_driver)
                        .ExecuteScript("return document.body.scrollHeight");
                    
                    // 高さが変わらなければ、これ以上新しいコンテンツはない
                    if (newHeight == lastHeight)
                    {
                        break;
                    }
                    
                    lastHeight = newHeight;
                }
                
                return _driver.PageSource;
            }, cancellationToken);
        }

        /// <summary>
        /// スクリーンショットを取得（デバッグ用）
        /// </summary>
        /// <param name="filePath">保存先ファイルパス</param>
        public void TakeScreenshot(string filePath)
        {
            var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
            screenshot.SaveAsFile(filePath);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _driver?.Quit();
                _driver?.Dispose();
                _disposed = true;
            }
        }
    }

    /// <summary>
    /// 動的コンテンツ抽出の設定オプション
    /// </summary>
    public class DynamicExtractionOptions
    {
        /// <summary>
        /// ヘッドレスモードで実行するか
        /// </summary>
        public bool Headless { get; set; } = true;

        /// <summary>
        /// ページロードのタイムアウト時間（秒）
        /// </summary>
        public int PageLoadTimeoutSeconds { get; set; } = 30;

        /// <summary>
        /// 動的コンテンツの読み込み完了後の追加待機時間（秒）
        /// </summary>
        public double AdditionalWaitSeconds { get; set; } = 2.0;

        /// <summary>
        /// カスタムユーザーエージェント
        /// </summary>
        public string UserAgent { get; set; } = string.Empty;
    }
}