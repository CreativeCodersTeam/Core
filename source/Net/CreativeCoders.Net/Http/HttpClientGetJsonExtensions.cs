using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Http
{
    [PublicAPI]
    public static class HttpClientGetJsonExtensions
    {
        public static async Task<T> GetJsonAsync<T>(this HttpClient httpClient, Uri requestUri,
            HttpCompletionOption completionOption, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            var response = await HttpClientExtensions
                .ExecuteHttpActionWithEnsureSuccessStatus(httpClient.GetAsync(requestUri, completionOption, cancellationToken, setupRequest))
                .ConfigureAwait(false);

            var dataObject = await response.Content.ReadAsJsonAsync<T>()
                .ConfigureAwait(false);

            return dataObject;
        }

        public static Task<T> GetJsonAsync<T>(this HttpClient httpClient, Uri requestUri, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            return httpClient.GetJsonAsync<T>(requestUri, HttpCompletionOption.ResponseContentRead, cancellationToken,
                setupRequest);
        }

        public static Task<T> GetJsonAsync<T>(this HttpClient httpClient, Uri requestUri, Action<HttpRequestMessage> setupRequest)
        {
            return httpClient.GetJsonAsync<T>(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None,
                setupRequest);
        }
    }
}