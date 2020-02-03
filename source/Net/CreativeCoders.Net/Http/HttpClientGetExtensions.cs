using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace CreativeCoders.Net.Http
{
    public static class HttpClientGetExtensions
    {
        public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, Uri requestUri,
            HttpCompletionOption completionOption, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            setupRequest?.Invoke(request);

            return httpClient.SendAsync(request, completionOption, cancellationToken);
        }

        public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, Uri requestUri,
            CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            return httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseContentRead, cancellationToken, setupRequest);
        }

        public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, Uri requestUri, Action<HttpRequestMessage> setupRequest)
        {
            return httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseContentRead, CancellationToken.None, setupRequest);
        }
    }
}