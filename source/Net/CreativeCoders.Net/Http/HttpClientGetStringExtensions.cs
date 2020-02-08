using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Http
{
    [PublicAPI]
    public static class HttpClientGetStringExtensions
    {
        public static async Task<string> GetStringAsync(this HttpClient httpClient, Uri requestUri,
            HttpCompletionOption completionOption, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            var response = await httpClient.GetAsync(requestUri, completionOption, cancellationToken, setupRequest)
                .ConfigureAwait(false);

            return await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
        }

        public static async Task<string> GetStringAsync(this HttpClient httpClient, Uri requestUri,
            CancellationToken cancellationToken, Action<HttpRequestMessage> setupRequest)
        {
            var response = await httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseContentRead, cancellationToken, setupRequest)
                .ConfigureAwait(false);

            return await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
        }

        public static async Task<string> GetStringAsync(this HttpClient httpClient, Uri requestUri,
            Action<HttpRequestMessage> setupRequest)
        {
            var response = await httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None, setupRequest)
                .ConfigureAwait(false);

            return await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
        }
    }
}