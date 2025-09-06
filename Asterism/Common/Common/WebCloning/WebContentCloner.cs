using System;
using System.Threading;
using System.Threading.Tasks;
using Asterism.Common.Network;
using Asterism.Common.WebCloning.Html;
using Asterism.Common.WebCloning.Models;

namespace Asterism.Common.WebCloning
{
    /// <summary>
    /// Main class for web content cloning with HTML parsing capabilities
    /// </summary>
    public class WebContentCloner : IDisposable
    {
        private readonly WebConnecter _webConnecter;
        private readonly HtmlParser _htmlParser;
        private readonly ExtractionConfig _defaultConfig;
        private bool _disposed;

        public WebContentCloner(ExtractionConfig config = null)
        {
            _webConnecter = new WebConnecter();
            _defaultConfig = config ?? CreateDefaultConfig();
            _htmlParser = new HtmlParser(_defaultConfig);
        }

        /// <summary>
        /// Clone web content from the specified URL
        /// </summary>
        /// <param name="url">URL to clone</param>
        /// <param name="config">Optional extraction configuration, uses default if null</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WebContent with parsed data</returns>
        public async Task<WebContent> CloneAsync(string url, ExtractionConfig config = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL cannot be null or empty", nameof(url));

            if (_disposed)
                throw new ObjectDisposedException(nameof(WebContentCloner));

            try
            {
                // Fetch HTML content
                var html = await _webConnecter.GetAsync(url, cancellationToken);
                
                // Create web content object
                var webContent = new WebContent
                {
                    Url = url,
                    RawHtml = html,
                    FetchedAt = DateTime.Now
                };

                // Use provided config or default
                var activeConfig = config ?? _defaultConfig;
                var parser = config != null ? new HtmlParser(config) : _htmlParser;

                // Parse HTML and extract elements
                webContent.Elements = parser.ParseHtml(html);

                // Extract metadata
                webContent.Metadata = parser.ExtractMetadata(html);

                // Extract title from elements if available
                var titleElement = webContent.Elements.Find(e => e.TagName == "title");
                if (titleElement != null)
                {
                    webContent.Title = titleElement.InnerText;
                }

                return webContent;
            }
            catch (Exception ex)
            {
                throw new WebCloningException($"Failed to clone content from {url}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Clone content and extract only specific tags
        /// </summary>
        /// <param name="url">URL to clone</param>
        /// <param name="targetTags">Specific tags to extract</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WebContent with only specified tags</returns>
        public async Task<WebContent> CloneSpecificTagsAsync(string url, string[] targetTags, CancellationToken cancellationToken = default)
        {
            var config = new ExtractionConfig();
            config.TargetTags.AddRange(targetTags);
            
            return await CloneAsync(url, config, cancellationToken);
        }

        /// <summary>
        /// Clone content and extract text only (no HTML)
        /// </summary>
        /// <param name="url">URL to clone</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>WebContent with text content only</returns>
        public async Task<WebContent> CloneTextOnlyAsync(string url, CancellationToken cancellationToken = default)
        {
            var config = new ExtractionConfig
            {
                ExtractText = true,
                ExtractHtml = false,
                ExtractAttributes = false
            };
            
            return await CloneAsync(url, config, cancellationToken);
        }

        /// <summary>
        /// Create default extraction configuration
        /// </summary>
        private ExtractionConfig CreateDefaultConfig()
        {
            return new ExtractionConfig
            {
                ExtractText = true,
                ExtractHtml = false,
                ExtractAttributes = true,
                MaxElements = 100
            };
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                // WebConnecter has its own disposal logic in finalizer
                _disposed = true;
            }
        }
    }

    /// <summary>
    /// Exception thrown during web cloning operations
    /// </summary>
    public class WebCloningException : Exception
    {
        public WebCloningException(string message) : base(message) { }
        public WebCloningException(string message, Exception innerException) : base(message, innerException) { }
    }
}