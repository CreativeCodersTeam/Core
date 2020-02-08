using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Http
{
    [PublicAPI]
    public static class HttpClientPutExtensions
    {
        public static Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, Uri requestUri,
            HttpContent content, HttpCompletionOption completionOption, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, requestUri) { Content = content };
            setupRequest?.Invoke(request);

            return httpClient.SendAsync(request, completionOption, cancellationToken);
        }

        public static Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, Uri requestUri,
            HttpContent content, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            return httpClient.PutAsync(requestUri, content, HttpCompletionOption.ResponseContentRead,
                cancellationToken, setupRequest);
        }

        public static Task<HttpResponseMessage> PutAsync(this HttpClient httpClient, Uri requestUri,
            HttpContent content, Action<HttpRequestMessage> setupRequest)
        {
            return httpClient.PutAsync(requestUri, content, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None, setupRequest);
        }

        public static async Task<string> PutReturnStringAsync(this HttpClient httpClient, Uri requestUri,
            HttpContent content, HttpCompletionOption completionOption, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            var response = await HttpClientExtensions.ExecuteHttpActionWithEnsureSuccessStatus(
                    httpClient.PutAsync(requestUri, content, completionOption, cancellationToken, setupRequest))
                .ConfigureAwait(false);

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public static Task<string> PutReturnStringAsync(this HttpClient httpClient, Uri requestUri,
            HttpContent content, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            return httpClient.PutReturnStringAsync(requestUri, content, HttpCompletionOption.ResponseContentRead,
                cancellationToken, setupRequest);
        }

        public static Task<string> PutReturnStringAsync(this HttpClient httpClient, Uri requestUri,
            HttpContent content, Action<HttpRequestMessage> setupRequest)
        {
            return httpClient.PutReturnStringAsync(requestUri, content, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None, setupRequest);
        }

        public static async Task<T> PutReturnJsonAsync<T>(this HttpClient httpClient, Uri requestUri,
            HttpContent content, HttpCompletionOption completionOption, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            var response = await HttpClientExtensions.ExecuteHttpActionWithEnsureSuccessStatus(
                    httpClient.PutAsync(requestUri, content, completionOption, cancellationToken, setupRequest))
                .ConfigureAwait(false);

            return await response.Content.ReadAsJsonAsync<T>().ConfigureAwait(false);
        }

        public static Task<T> PutReturnJsonAsync<T>(this HttpClient httpClient, Uri requestUri,
            HttpContent content, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            return httpClient.PutReturnJsonAsync<T>(requestUri, content, HttpCompletionOption.ResponseContentRead,
                cancellationToken, setupRequest);
        }

        public static Task<T> PutReturnStringAsync<T>(this HttpClient httpClient, Uri requestUri,
            HttpContent content, Action<HttpRequestMessage> setupRequest)
        {
            return httpClient.PutReturnJsonAsync<T>(requestUri, content, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None, setupRequest);
        }
    }
}