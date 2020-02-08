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
    public static class HttpClientPutJsonExtensions
    {
        public static Task<HttpResponseMessage> PutJsonAsync<T>(this HttpClient httpClient, Uri requestUri,
            T dataObject, HttpCompletionOption completionOption, CancellationToken cancellationToken,
            Action<HttpRequestMessage> setupRequest)
        {
            var content = new StringContent(JsonConvert.SerializeObject(dataObject),
                Encoding.UTF8, ContentMediaTypes.Application.Json);

            return httpClient.PutAsync(requestUri, content, completionOption, cancellationToken, setupRequest);
        }

        public static Task<HttpResponseMessage> PutJsonAsync<T>(this HttpClient httpClient, Uri requestUri, T dataObject)
        {
            return httpClient.PutJsonAsync(requestUri, dataObject, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None, null);
        }

        public static Task<HttpResponseMessage> PutJsonAsync<T>(this HttpClient httpClient, Uri requestUri,
            CancellationToken cancellationToken, T dataObject)
        {
            return httpClient.PutJsonAsync(requestUri, dataObject, HttpCompletionOption.ResponseContentRead,
                cancellationToken, null);
        }

        public static Task<HttpResponseMessage> PutJsonAsync<T>(this HttpClient httpClient, Uri requestUri,
            HttpCompletionOption completionOption, CancellationToken cancellationToken, T dataObject)
        {
            return httpClient.PutJsonAsync(requestUri, dataObject, completionOption,
                cancellationToken, null);
        }

        public static Task<HttpResponseMessage> PutJsonAsync<T>(this HttpClient httpClient, Uri requestUri,
            T dataObject, Action<HttpRequestMessage> setupRequestMessage)
        {
            return httpClient.PutJsonAsync(requestUri, dataObject, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None, setupRequestMessage);
        }

        public static Task<HttpResponseMessage> PutJsonAsync<T>(this HttpClient httpClient, Uri requestUri,
            CancellationToken cancellationToken, T dataObject, Action<HttpRequestMessage> setupRequestMessage)
        {
            return httpClient.PutJsonAsync(requestUri, dataObject, HttpCompletionOption.ResponseContentRead,
                cancellationToken, setupRequestMessage);
        }

        public static Task<HttpResponseMessage> PutJsonAsync<T>(this HttpClient httpClient, Uri requestUri,
            HttpCompletionOption completionOption, CancellationToken cancellationToken, T dataObject,
            Action<HttpRequestMessage> setupRequestMessage)
        {
            return httpClient.PutJsonAsync(requestUri, dataObject, completionOption,
                cancellationToken, setupRequestMessage);
        }

        public static async Task<TResult> PutJsonAsync<T, TResult>(this HttpClient httpClient, Uri requestUri,
            HttpCompletionOption completionOption, CancellationToken cancellationToken, T dataObject,
            Action<HttpRequestMessage> setupRequestMessage)
            where TResult : class
        {
            var response = await HttpClientExtensions
                .ExecuteHttpActionWithEnsureSuccessStatus(httpClient.PutJsonAsync(requestUri, completionOption,
                    cancellationToken, dataObject, setupRequestMessage))
                .ConfigureAwait(false);

            return await response.Content.ReadAsJsonAsync<TResult>().ConfigureAwait(false);
        }

        public static Task<TResult> PutJsonAsync<T, TResult>(this HttpClient httpClient, Uri requestUri,
            CancellationToken cancellationToken, T dataObject, Action<HttpRequestMessage> setupRequestMessage)
            where TResult : class
        {
            return httpClient.PutJsonAsync<T, TResult>(requestUri, HttpCompletionOption.ResponseContentRead,
                cancellationToken, dataObject, setupRequestMessage);
        }

        public static Task<TResult> PutJsonAsync<T, TResult>(this HttpClient httpClient, Uri requestUri, T dataObject,
            Action<HttpRequestMessage> setupRequestMessage)
            where TResult : class
        {
            return httpClient.PutJsonAsync<T, TResult>(requestUri, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None, dataObject, setupRequestMessage);
        }

        public static Task<TResult> PutJsonAsync<T, TResult>(this HttpClient httpClient, Uri requestUri,
            HttpCompletionOption completionOption, CancellationToken cancellationToken, T dataObject)
            where TResult : class
        {
            return httpClient.PutJsonAsync<T, TResult>(requestUri, completionOption,
                cancellationToken, dataObject, null);
        }

        public static Task<TResult> PutJsonAsync<T, TResult>(this HttpClient httpClient, Uri requestUri,
            CancellationToken cancellationToken, T dataObject)
            where TResult : class
        {
            return httpClient.PutJsonAsync<T, TResult>(requestUri, HttpCompletionOption.ResponseContentRead,
                cancellationToken, dataObject, null);
        }

        public static Task<TResult> PutJsonAsync<T, TResult>(this HttpClient httpClient, Uri requestUri, T dataObject)
            where TResult : class
        {
            return httpClient.PutJsonAsync<T, TResult>(requestUri, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None, dataObject, null);
        }

        public static async Task<object> PutJsonAsync(this HttpClient httpClient, Type resultType, Uri requestUri,
            HttpCompletionOption completionOption, CancellationToken cancellationToken, object dataObject,
            Action<HttpRequestMessage> setupRequestMessage)
        {
            var response =
                await httpClient.PutJsonAsync(requestUri, completionOption, cancellationToken, dataObject,
                        setupRequestMessage)
                    .ConfigureAwait(false);

            return await response.Content.ReadAsJsonAsync(resultType);
        }

        public static Task<object> PutJsonAsync(this HttpClient httpClient, Type resultType, Uri requestUri,
            CancellationToken cancellationToken, object dataObject,
            Action<HttpRequestMessage> setupRequestMessage)
        {
            return httpClient.PutJsonAsync(resultType, requestUri, HttpCompletionOption.ResponseContentRead,
                cancellationToken, dataObject, setupRequestMessage);
        }

        public static Task<object> PutJsonAsync(this HttpClient httpClient, Type resultType, Uri requestUri,
            object dataObject, Action<HttpRequestMessage> setupRequestMessage)
        {
            return httpClient.PutJsonAsync(resultType, requestUri, HttpCompletionOption.ResponseContentRead,
                CancellationToken.None, dataObject, setupRequestMessage);
        }

        public static Task<object> PutJsonAsync(this HttpClient httpClient, Type resultType, Uri requestUri,
            HttpCompletionOption completionOption, CancellationToken cancellationToken, object dataObject)
        {
            return httpClient.PutJsonAsync(resultType, requestUri, completionOption, cancellationToken, dataObject, null);
        }

        public static Task<object> PutJsonAsync(this HttpClient httpClient, Type resultType, Uri requestUri,
            CancellationToken cancellationToken, object dataObject)
        {
            return httpClient.PutJsonAsync(resultType, requestUri, cancellationToken, dataObject, null);
        }

        public static Task<object> PutJsonAsync(this HttpClient httpClient, Type resultType, Uri requestUri,
            object dataObject)
        {
            return httpClient.PutJsonAsync(resultType, requestUri, dataObject, null);
        }
    }
}