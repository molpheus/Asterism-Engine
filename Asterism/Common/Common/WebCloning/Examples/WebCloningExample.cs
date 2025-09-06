using System;
using System.Threading.Tasks;
using Asterism.Common.WebCloning;
using Asterism.Common.WebCloning.Models;

namespace Asterism.Common.WebCloning.Examples
{
    /// <summary>
    /// Example usage of WebContentCloner functionality
    /// </summary>
    public class WebCloningExample
    {
        /// <summary>
        /// Demonstrate basic web content cloning
        /// </summary>
        public static async Task BasicCloningExample()
        {
            using (var cloner = new WebContentCloner())
            {
                try
                {
                    // Clone a webpage
                    var content = await cloner.CloneAsync("https://example.com");
                    
                    Console.WriteLine($"Title: {content.Title}");
                    Console.WriteLine($"URL: {content.Url}");
                    Console.WriteLine($"Fetched at: {content.FetchedAt}");
                    Console.WriteLine($"Elements found: {content.Elements.Count}");
                    
                    // Display first few elements
                    for (int i = 0; i < Math.Min(5, content.Elements.Count); i++)
                    {
                        var element = content.Elements[i];
                        Console.WriteLine($"  {element.TagName}: {element.InnerText?.Substring(0, Math.Min(50, element.InnerText.Length ?? 0))}...");
                    }
                }
                catch (WebCloningException ex)
                {
                    Console.WriteLine($"Cloning failed: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Demonstrate cloning with specific tags
        /// </summary>
        public static async Task SpecificTagsCloningExample()
        {
            using (var cloner = new WebContentCloner())
            {
                try
                {
                    // Clone only headings and links
                    var content = await cloner.CloneSpecificTagsAsync("https://example.com", 
                        new[] { "h1", "h2", "h3", "a" });
                    
                    Console.WriteLine($"Found {content.Elements.Count} heading and link elements:");
                    
                    foreach (var element in content.Elements)
                    {
                        Console.WriteLine($"  {element.TagName}: {element.InnerText}");
                        
                        // Show href attribute for links
                        if (element.TagName == "a" && element.Attributes.ContainsKey("href"))
                        {
                            Console.WriteLine($"    -> {element.Attributes["href"]}");
                        }
                    }
                }
                catch (WebCloningException ex)
                {
                    Console.WriteLine($"Cloning failed: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Demonstrate custom configuration cloning
        /// </summary>
        public static async Task CustomConfigCloningExample()
        {
            var config = new ExtractionConfig
            {
                ExtractText = true,
                ExtractHtml = true,
                ExtractAttributes = true,
                MaxElements = 50
            };
            
            // Add specific tags to extract
            config.TargetTags.Add("title");
            config.TargetTags.Add("h1");
            config.TargetTags.Add("p");
            config.TargetTags.Add("img");

            using (var cloner = new WebContentCloner(config))
            {
                try
                {
                    var content = await cloner.CloneAsync("https://example.com");
                    
                    Console.WriteLine($"Custom cloning results for {content.Url}:");
                    Console.WriteLine($"Metadata entries: {content.Metadata.Count}");
                    
                    // Show metadata
                    foreach (var meta in content.Metadata)
                    {
                        Console.WriteLine($"  {meta.Key}: {meta.Value}");
                    }
                    
                    // Show elements with attributes
                    foreach (var element in content.Elements)
                    {
                        Console.WriteLine($"\n{element.TagName} (Position: {element.Position}):");
                        Console.WriteLine($"  Text: {element.InnerText?.Substring(0, Math.Min(100, element.InnerText.Length ?? 0))}");
                        
                        if (element.Attributes.Count > 0)
                        {
                            Console.WriteLine("  Attributes:");
                            foreach (var attr in element.Attributes)
                            {
                                Console.WriteLine($"    {attr.Key}=\"{attr.Value}\"");
                            }
                        }
                    }
                }
                catch (WebCloningException ex)
                {
                    Console.WriteLine($"Cloning failed: {ex.Message}");
                }
            }
        }
    }
}