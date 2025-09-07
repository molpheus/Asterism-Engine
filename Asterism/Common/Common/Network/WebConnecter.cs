using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Asterism.Common.Network
{
    public class WebConnecter
    {
        public int MaxConnections { get; set; } = 10;

        public Stack<HttpClient> Connections { get; set; } = new Stack<HttpClient>();

        private HttpMessageHandler _handler = new HttpClientHandler()
        {
            AllowAutoRedirect = true,
            UseCookies = true,
            UseDefaultCredentials = false,
            PreAuthenticate = false,
            MaxConnectionsPerServer = 10,
        };

        public WebConnecter()
        {
            for (int i = 0; i < MaxConnections; i++)
            {
                Connections.Push(new HttpClient(_handler, false));
            }
        }

        ~WebConnecter()
        {
            foreach (var client in Connections)
            {
                client.Dispose();
            }
        }


        public async Task<string> GetAsync(string url, CancellationToken token = default)
        {
            HttpClient client = null;
            try
            {
                if (Connections.TryPop(out client))
                {
                    var response = await client.GetAsync(url, token);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new InvalidOperationException("No available HTTP connections");
                }
            }
            finally
            {
                if (client != null)
                {
                    Connections.Push(client);
                }
            }
        }

        public async Task<string> PostAsync(string url, HttpContent content, CancellationToken token = default)
        {
            HttpClient client = null;
            try
            {
                if (Connections.TryPop(out client))
                {
                    var response = await client.PostAsync(url, content, token);
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    throw new InvalidOperationException("No available HTTP connections");
                }
            }
            finally
            {
                if (client != null)
                {
                    Connections.Push(client);
                }
            }
        }
    }
}
