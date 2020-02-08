using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace CreativeCoders.Net.Http
{
    [PublicAPI]
    public static class HttpClientPostJsonExtensions
    {
        public static Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient httpClient, Uri requestUri,
            T dataObject, HttpCompletionOption completionOption, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dataObject),
                Encoding.UTF8, ContentMediaTypes.Application.Json);

            return httpClient.PostAsync(requestUri, content, completionOption, cancellationToken, setupRequest);
        }

        public static Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient httpClient, Uri requestUri, T dataObject)
        {
            return httpClient.PostJsonAsync(requestUri, dataObject, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None, null);
        }

        public static Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient httpClient, Uri requestUri,
            CancellationToken cancellationToken, T dataObject)
        {
            return httpClient.PostJsonAsync(requestUri, dataObject, HttpCompletionOption.ResponseContentRead,
                cancellationToken, null);
        }

        public static Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient httpClient, Uri requestUri,
            HttpCompletionOption completionOption, CancellationToken cancellationToken, T dataObject)
        {
            return httpClient.PostJsonAsync(requestUri, dataObject, completionOption,
                cancellationToken, null);
        }

        public static Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient httpClient, Uri requestUri,
            T dataObject, Action<HttpRequestMessage> setupRequestMessage)
        {
            return httpClient.PostJsonAsync(requestUri, dataObject, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None, setupRequestMessage);
        }

        public static Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient httpClient, Uri requestUri,
            CancellationToken cancellationToken, T dataObject, Action<HttpRequestMessage> setupRequestMessage)
        {
            return httpClient.PostJsonAsync(requestUri, dataObject, HttpCompletionOption.ResponseContentRead,
                cancellationToken, setupRequestMessage);
        }

        public static Task<HttpResponseMessage> PostJsonAsync<T>(this HttpClient httpClient, Uri requestUri,
            HttpCompletionOption completionOption, CancellationToken cancellationToken, T dataObject,
            Action<HttpRequestMessage> setupRequestMessage)
        {
            return httpClient.PostJsonAsync(requestUri, dataObject, completionOption,
                cancellationToken, setupRequestMessage);
        }

        public static async Task<TResult> PostJsonAsync<T, TResult>(this HttpClient httpClient, Uri requestUri,
            HttpCompletionOption completionOption, CancellationToken cancellationToken, T dataObject,
            Action<HttpRequestMessage> setupRequestMessage)
            where TResult : class
        {
            var response = await HttpClientExtensions
                .ExecuteHttpActionWithEnsureSuccessStatus(httpClient.PostJsonAsync(requestUri, completionOption,
                    cancellationToken, dataObject, setupRequestMessage))
                .ConfigureAwait(false);

            return await response.Content.ReadAsJsonAsync<TResult>().ConfigureAwait(false);
        }

        public static Task<TResult> PostJsonAsync<T, TResult>(this HttpClient httpClient, Uri requestUri,
            CancellationToken cancellationToken, T dataObject, Action<HttpRequestMessage> setupRequestMessage)
            where TResult : class
        {
            return httpClient.PostJsonAsync<T, TResult>(requestUri, HttpCompletionOption.ResponseContentRead,
                cancellationToken, dataObject, setupRequestMessage);
        }

        public static Task<TResult> PostJsonAsync<T, TResult>(this HttpClient httpClient, Uri requestUri, T dataObject,
            Action<HttpRequestMessage> setupRequestMessage)
            where TResult : class
        {
            return httpClient.PostJsonAsync<T, TResult>(requestUri, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None, dataObject, setupRequestMessage);
        }

        public static Task<TResult> PostJsonAsync<T, TResult>(this HttpClient httpClient, Uri requestUri,
            HttpCompletionOption completionOption, CancellationToken cancellationToken, T dataObject)
            where TResult : class
        {
            return httpClient.PostJsonAsync<T, TResult>(requestUri, completionOption,
                cancellationToken, dataObject, null);
        }

        public static Task<TResult> PostJsonAsync<T, TResult>(this HttpClient httpClient, Uri requestUri,
            CancellationToken cancellationToken, T dataObject)
            where TResult : class
        {
            return httpClient.PostJsonAsync<T, TResult>(requestUri, HttpCompletionOption.ResponseContentRead,
                cancellationToken, dataObject, null);
        }

        public static Task<TResult> PostJsonAsync<T, TResult>(this HttpClient httpClient, Uri requestUri, T dataObject)
            where TResult : class
        {
            return httpClient.PostJsonAsync<T, TResult>(requestUri, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None, dataObject, null);
        }

        public static async Task<object> PostJsonAsync(this HttpClient httpClient, Type resultType, Uri requestUri,
            HttpCompletionOption completionOption, CancellationToken cancellationToken, object dataObject,
            Action<HttpRequestMessage> setupRequestMessage)
        {
            var response =
                await httpClient.PostJsonAsync(requestUri, completionOption, cancellationToken, dataObject, setupRequestMessage)
                    .ConfigureAwait(false);

            return await response.Content.ReadAsJsonAsync(resultType)
                .ConfigureAwait(false);
        }

        public static Task<object> PostJsonAsync(this HttpClient httpClient, Type resultType, Uri requestUri,
            CancellationToken cancellationToken, object dataObject,
            Action<HttpRequestMessage> setupRequestMessage)
        {
            return httpClient.PostJsonAsync(resultType, requestUri, HttpCompletionOption.ResponseContentRead,
                cancellationToken, dataObject, setupRequestMessage);
        }

        public static Task<object> PostJsonAsync(this HttpClient httpClient, Type resultType, Uri requestUri,
            object dataObject, Action<HttpRequestMessage> setupRequestMessage)
        {
            return httpClient.PostJsonAsync(resultType, requestUri, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None, dataObject, setupRequestMessage);
        }

        public static Task<object> PostJsonAsync(this HttpClient httpClient, Type resultType, Uri requestUri,
            HttpCompletionOption completionOption, CancellationToken cancellationToken, object dataObject)
        {
            return httpClient.PostJsonAsync(resultType, requestUri, completionOption, cancellationToken, dataObject, null);
        }

        public static Task<object> PostJsonAsync(this HttpClient httpClient, Type resultType, Uri requestUri,
            CancellationToken cancellationToken, object dataObject)
        {
            return httpClient.PostJsonAsync(resultType, requestUri, cancellationToken, dataObject, null);
        }

        public static Task<object> PostJsonAsync(this HttpClient httpClient, Type resultType, Uri requestUri,
            object dataObject)
        {
            return httpClient.PostJsonAsync(resultType, requestUri, dataObject, null);
        }
    }
}