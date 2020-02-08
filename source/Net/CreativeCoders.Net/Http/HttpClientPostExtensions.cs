using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Http
{
    [PublicAPI]
    public static class HttpClientPostExtensions
    {
        public static Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, Uri requestUri,
            HttpContent content, HttpCompletionOption completionOption, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri) {Content = content};
            setupRequest?.Invoke(request);

            return httpClient.SendAsync(request, completionOption, cancellationToken);
        }

        public static Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, Uri requestUri,
            HttpContent content, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            return httpClient.PostAsync(requestUri, content, HttpCompletionOption.ResponseContentRead,
                cancellationToken, setupRequest);
        }

        public static Task<HttpResponseMessage> PostAsync(this HttpClient httpClient, Uri requestUri,
            HttpContent content, Action<HttpRequestMessage> setupRequest)
        {
            return httpClient.PostAsync(requestUri, content, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None, setupRequest);
        }

        public static async Task<string> PostReturnStringAsync(this HttpClient httpClient, Uri requestUri,
            HttpContent content, HttpCompletionOption completionOption, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            var response = await HttpClientExtensions.ExecuteHttpActionWithEnsureSuccessStatus(
                    httpClient.PostAsync(requestUri, content, completionOption, cancellationToken, setupRequest))
                .ConfigureAwait(false);

            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public static Task<string> PostReturnStringAsync(this HttpClient httpClient, Uri requestUri,
            HttpContent content, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            return httpClient.PostReturnStringAsync(requestUri, content, HttpCompletionOption.ResponseContentRead,
                cancellationToken, setupRequest);
        }

        public static Task<string> PostReturnStringAsync(this HttpClient httpClient, Uri requestUri,
            HttpContent content, Action<HttpRequestMessage> setupRequest)
        {
            return httpClient.PostReturnStringAsync(requestUri, content, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None, setupRequest);
        }

        public static async Task<T> PostReturnJsonAsync<T>(this HttpClient httpClient, Uri requestUri,
            HttpContent content, HttpCompletionOption completionOption, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            var response = await HttpClientExtensions.ExecuteHttpActionWithEnsureSuccessStatus(
                    httpClient.PostAsync(requestUri, content, completionOption, cancellationToken, setupRequest))
                .ConfigureAwait(false);

            return await response.Content.ReadAsJsonAsync<T>().ConfigureAwait(false);
        }

        public static Task<T> PostReturnJsonAsync<T>(this HttpClient httpClient, Uri requestUri,
            HttpContent content, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            return httpClient.PostReturnJsonAsync<T>(requestUri, content, HttpCompletionOption.ResponseContentRead,
                cancellationToken, setupRequest);
        }

        public static Task<T> PostReturnStringAsync<T>(this HttpClient httpClient, Uri requestUri,
            HttpContent content, Action<HttpRequestMessage> setupRequest)
        {
            return httpClient.PostReturnJsonAsync<T>(requestUri, content, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None, setupRequest);
        }
    }
}