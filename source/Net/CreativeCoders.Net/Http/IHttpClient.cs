using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Net.Http.Auth;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Http
{
    [PublicAPI]
    public interface IHttpClient : IDisposable
    {
        Task<HttpResponseMessage> GetAsync(Uri requestUri);

        Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken);

        Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken);

        Task<string> GetStringAsync(Uri requestUri);

        Task<string> GetStringAsync(Uri requestUri, CancellationToken cancellationToken);

        Task<string> GetStringAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken);

        Task<T> GetJsonAsync<T>(Uri requestUri);

        Task<T> GetJsonAsync<T>(Uri requestUri, CancellationToken cancellationToken);

        Task<T> GetJsonAsync<T>(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken);

        Task<HttpResponseMessage> PostJsonAsync<T>(Uri requestUri, T postData);

        Task<HttpResponseMessage> PostJsonAsync<T>(Uri requestUri, T postData, CancellationToken cancellationToken);

        Task<HttpResponseMessage> PostJsonAsync<T>(Uri requestUri, T postData, HttpCompletionOption completionOption, CancellationToken cancellationToken);

        Task<TResult> PostJsonAsync<T, TResult>(Uri requestUri, T dataObject)
            where TResult : class;

        Task<TResult> PostJsonAsync<T, TResult>(Uri requestUri, T dataObject, CancellationToken cancellationToken)
            where TResult : class;

        Task<TResult> PostJsonAsync<T, TResult>(Uri requestUri, T dataObject, HttpCompletionOption completionOption, CancellationToken cancellationToken)
            where TResult : class;

        Task<HttpResponseMessage> PostAsync(Uri requestUri);

        Task<HttpResponseMessage> PostAsync(Uri requestUri, CancellationToken cancellationToken);

        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken);

        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content);

        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken);

        Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, HttpCompletionOption completionOption, CancellationToken cancellationToken);

        Task<TResult> PostReturnJsonAsync<TResult>(Uri requestUri, HttpContent content)
            where TResult : class;

        Task<TResult> PostReturnJsonAsync<TResult>(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
            where TResult : class;

        Task<TResult> PostReturnJsonAsync<TResult>(Uri requestUri, HttpContent content, HttpCompletionOption completionOption, CancellationToken cancellationToken)
            where TResult : class;

        Task<string> PostReturnStringAsync(Uri requestUri);

        Task<string> PostReturnStringAsync(Uri requestUri, CancellationToken cancellationToken);

        Task<string> PostReturnStringAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken);

        Task<TResult> PostReturnJsonAsync<TResult>(Uri requestUri)
            where TResult : class;

        Task<TResult> PostReturnJsonAsync<TResult>(Uri requestUri, CancellationToken cancellationToken)
            where TResult : class;

        Task<TResult> PostReturnJsonAsync<TResult>(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
            where TResult : class;

        Task<HttpResponseMessage> PutJsonAsync<T>(Uri requestUri, T postData);

        Task<HttpResponseMessage> PutJsonAsync<T>(Uri requestUri, T postData, CancellationToken cancellationToken);

        Task<HttpResponseMessage> PutJsonAsync<T>(Uri requestUri, T postData, HttpCompletionOption completionOption, CancellationToken cancellationToken);

        Task<TResult> PutJsonAsync<T, TResult>(Uri requestUri, T dataObject)
            where TResult : class;

        Task<TResult> PutJsonAsync<T, TResult>(Uri requestUri, T dataObject, CancellationToken cancellationToken)
            where TResult : class;

        Task<TResult> PutJsonAsync<T, TResult>(Uri requestUri, T dataObject, HttpCompletionOption completionOption, CancellationToken cancellationToken)
            where TResult : class;

        Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, HttpCompletionOption completionOption,
            CancellationToken cancellationToken);

        IHttpAuthenticator Authenticator { get; set; }
    }
}