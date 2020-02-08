using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Http
{
    [PublicAPI]
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostStringAsync(this HttpClient httpClient, Uri requestUri,
            HttpCompletionOption completionOption, CancellationToken cancellationToken, string content,
            string mediaType, Action<HttpRequestMessage> setupRequestMessage)
        {
            var httpPostRequest = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(content, Encoding.UTF8, mediaType)
            };
            setupRequestMessage(httpPostRequest);

            var response = httpClient.SendAsync(httpPostRequest, completionOption, cancellationToken);

            return response;
        }

        public static Task<HttpResponseMessage> PostStringAsync(this HttpClient httpClient, Uri requestUri, CancellationToken cancellationToken, string content, string mediaType, Action<HttpRequestMessage> setupRequestMessage)
        {
            return httpClient.PostStringAsync(requestUri, HttpCompletionOption.ResponseContentRead,
                cancellationToken, content, mediaType, setupRequestMessage);
        }

        public static Task<HttpResponseMessage> PostStringAsync(this HttpClient httpClient, Uri requestUri, string content, string mediaType, Action<HttpRequestMessage> setupRequestMessage)
        {
            return httpClient.PostStringAsync(requestUri, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None, content, mediaType, setupRequestMessage);
        }

        //public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, Uri requestUri,
        //    HttpCompletionOption completionOption, CancellationToken cancellationToken,
        //    Action<HttpRequestMessage> setupHttpRequest)
        //{
        //    var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUri);
        //    setupHttpRequest?.Invoke(httpRequest);

        //    return httpClient.SendAsync(httpRequest, completionOption, cancellationToken);
        //}

        //public static Task<HttpResponseMessage> GetAsync(this HttpClient httpClient, Uri requestUri,
        //    CancellationToken cancellationToken, Action<HttpRequestMessage> setupHttpRequest)
        //{
        //    return httpClient.GetAsync(requestUri, HttpCompletionOption.ResponseContentRead, cancellationToken,
        //        setupHttpRequest);
        //}

        public static Task<HttpResponseMessage> SendAsync(this HttpClient httpClient, HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken, Action<HttpRequestMessage> setupRequestMessage)
        {
            setupRequestMessage(request);
            return httpClient.SendAsync(request, completionOption, cancellationToken);
        }

        public static async Task<HttpResponseMessage> ExecuteHttpActionWithEnsureSuccessStatus(Task<HttpResponseMessage> httpAction)
        {
            var response = await httpAction.ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return response;
        }
    }
}