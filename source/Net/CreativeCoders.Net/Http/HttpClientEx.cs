using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CreativeCoders.Net.Http.Auth;

namespace CreativeCoders.Net.Http
{
    public class HttpClientEx : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public HttpClientEx(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Authenticator = new NullHttpAuthenticator();
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        private void SetupHttpRequestMessage(HttpRequestMessage requestMessage)
        {
            Authenticator.PrepareHttpRequest(requestMessage);
        }

        private async Task<HttpResponseMessage> ExecuteHttpAction(Func<Task<HttpResponseMessage>> httpAction)
        {
            var response = await httpAction()
                .ConfigureAwait(false);

            // ReSharper disable once InvertIf
            if (response.StatusCode == HttpStatusCode.Unauthorized && Authenticator.CanAuthenticate())
            {
                await DoLogin().ConfigureAwait(false);

                return await httpAction().ConfigureAwait(false);
            }

            return response;
        }

        private Task<T> ExecuteJsonHttpAction<T>(Func<Task<HttpResponseMessage>> httpAction)
        {
            return ExecuteHttpActionReadContent(httpAction, content => content.ReadAsJsonAsync<T>());
        }

        private Task<string> ExecuteStringHttpAction(Func<Task<HttpResponseMessage>> httpAction)
        {
            return ExecuteHttpActionReadContent(httpAction, content => content.ReadAsStringAsync());
        }

        private async Task<T> ExecuteHttpActionReadContent<T>(Func<Task<HttpResponseMessage>> httpAction, Func<HttpContent, Task<T>> readContent)
        {
            var response = await ExecuteHttpAction(httpAction).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            return await readContent(response.Content).ConfigureAwait(false);
        }

        //private async Task<string> ExecuteJsonReturnStringHttpAction<T>(Func<HttpContent, Task<HttpResponseMessage>> httpAction, T dataObject)
        //{
        //    var jsonData = JsonConvert.SerializeObject(dataObject);
        //    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        //    var response = await httpAction(content)
        //        .ConfigureAwait(false);

        //    if (response.StatusCode == HttpStatusCode.Unauthorized)
        //    {
        //        await DoLogin().ConfigureAwait(false);

        //        response = await httpAction(content).ConfigureAwait(false);
        //    }

        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        //}

        //private async Task<TResult> ExecuteJsonReturnJsonHttpAction<T, TResult>(Func<HttpContent, Task<HttpResponseMessage>> httpAction, T dataObject)
        //{
        //    var jsonData = JsonConvert.SerializeObject(dataObject);
        //    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        //    var response = await httpAction(content)
        //        .ConfigureAwait(false);

        //    if (response.StatusCode == HttpStatusCode.Unauthorized)
        //    {
        //        await DoLogin().ConfigureAwait(false);

        //        response = await httpAction(content).ConfigureAwait(false);
        //    }

        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadAsJsonAsync<TResult>().ConfigureAwait(false);
        //}

        private Task DoLogin()
        {
            return Authenticator?.AuthenticateAsync();
        }

        public Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return ExecuteHttpAction(() =>
                _httpClient.GetAsync(requestUri, SetupHttpRequestMessage));
        }

        public Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            return ExecuteHttpAction(() =>
                _httpClient.GetAsync(requestUri, cancellationToken, SetupHttpRequestMessage));
        }

        public Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return ExecuteHttpAction(() =>
                _httpClient.GetAsync(requestUri, completionOption, cancellationToken, SetupHttpRequestMessage));
        }

        public Task<string> GetStringAsync(Uri requestUri)
        {
            return ExecuteStringHttpAction(() =>
                _httpClient.GetAsync(requestUri, SetupHttpRequestMessage));
        }

        public Task<string> GetStringAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            return ExecuteStringHttpAction(() =>
                _httpClient.GetAsync(requestUri, cancellationToken, SetupHttpRequestMessage));
        }

        public Task<string> GetStringAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return ExecuteStringHttpAction(() =>
                _httpClient.GetAsync(requestUri, completionOption, cancellationToken, SetupHttpRequestMessage));
        }

        public Task<T> GetJsonAsync<T>(Uri requestUri)
        {
            return ExecuteJsonHttpAction<T>(() =>
                _httpClient.GetAsync(requestUri, SetupHttpRequestMessage));
        }

        public Task<T> GetJsonAsync<T>(Uri requestUri, CancellationToken cancellationToken)
        {
            return ExecuteJsonHttpAction<T>(() =>
                _httpClient.GetAsync(requestUri, cancellationToken, SetupHttpRequestMessage));
        }

        public Task<T> GetJsonAsync<T>(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return ExecuteJsonHttpAction<T>(() =>
                _httpClient.GetAsync(requestUri, completionOption, cancellationToken, SetupHttpRequestMessage));
        }

        public Task<HttpResponseMessage> PostJsonAsync<T>(Uri requestUri, T postData)
        {
            return ExecuteHttpAction(() =>
                _httpClient.PostJsonAsync(requestUri, postData, SetupHttpRequestMessage));
        }

        public Task<HttpResponseMessage> PostJsonAsync<T>(Uri requestUri, T postData, CancellationToken cancellationToken)
        {
            return ExecuteHttpAction(() => _httpClient.PostJsonAsync(requestUri, cancellationToken, postData,
                SetupHttpRequestMessage));
        }

        public Task<HttpResponseMessage> PostJsonAsync<T>(Uri requestUri, T postData, HttpCompletionOption completionOption,
            CancellationToken cancellationToken)
        {
            return ExecuteHttpAction(() => _httpClient.PostJsonAsync(requestUri, completionOption, cancellationToken,
                postData, SetupHttpRequestMessage));
        }

        public Task<TResult> PostJsonAsync<T, TResult>(Uri requestUri, T dataObject) where TResult : class
        {
            return ExecuteJsonHttpAction<TResult>(() =>
                _httpClient.PostJsonAsync(requestUri, dataObject, SetupHttpRequestMessage));
        }

        public Task<TResult> PostJsonAsync<T, TResult>(Uri requestUri, T dataObject, CancellationToken cancellationToken) where TResult : class
        {
            return ExecuteJsonHttpAction<TResult>(() => _httpClient.PostJsonAsync(requestUri, cancellationToken,
                dataObject, SetupHttpRequestMessage));
        }

        public Task<TResult> PostJsonAsync<T, TResult>(Uri requestUri, T dataObject, HttpCompletionOption completionOption,
            CancellationToken cancellationToken) where TResult : class
        {
            return ExecuteJsonHttpAction<TResult>(() => _httpClient.PostJsonAsync(requestUri, completionOption,
                cancellationToken, dataObject, SetupHttpRequestMessage));
        }

        public Task<HttpResponseMessage> PostAsync(Uri requestUri)
        {
            return ExecuteHttpAction(() =>
                _httpClient.PostAsync(requestUri, SetupHttpRequestMessage));
        }

        public Task<HttpResponseMessage> PostAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            return ExecuteHttpAction(() =>
                _httpClient.PostAsync(requestUri, cancellationToken, SetupHttpRequestMessage));
        }

        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return ExecuteHttpAction(() =>
                _httpClient.PostAsync(requestUri, completionOption, cancellationToken,
                    SetupHttpRequestMessage));
        }

        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
        {
            return ExecuteHttpAction(() =>
                _httpClient.PostAsync(requestUri, content, SetupHttpRequestMessage));
        }

        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            return ExecuteHttpAction(() =>
                _httpClient.PostAsync(requestUri, content, cancellationToken, SetupHttpRequestMessage));
        }

        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, HttpCompletionOption completionOption,
            CancellationToken cancellationToken)
        {
            return ExecuteHttpAction(() =>
                _httpClient.PostAsync(requestUri, content, completionOption, cancellationToken,
                    SetupHttpRequestMessage));
        }

        public Task<TResult> PostReturnJsonAsync<TResult>(Uri requestUri, HttpContent content) where TResult : class
        {
            return ExecuteJsonHttpAction<TResult>(() =>
                _httpClient.PostAsync(requestUri, content, SetupHttpRequestMessage));
        }

        public Task<TResult> PostReturnJsonAsync<TResult>(Uri requestUri, HttpContent content, CancellationToken cancellationToken) where TResult : class
        {
            return ExecuteJsonHttpAction<TResult>(() =>
                _httpClient.PostAsync(requestUri, content, cancellationToken, SetupHttpRequestMessage));
        }

        public Task<TResult> PostReturnJsonAsync<TResult>(Uri requestUri, HttpContent content, HttpCompletionOption completionOption,
            CancellationToken cancellationToken) where TResult : class
        {
            return ExecuteJsonHttpAction<TResult>(() =>
                _httpClient.PostAsync(requestUri, content, completionOption, cancellationToken,
                    SetupHttpRequestMessage));
        }

        public Task<string> PostReturnStringAsync(Uri requestUri)
        {
            return ExecuteStringHttpAction(() =>
                _httpClient.PostAsync(requestUri, SetupHttpRequestMessage));
        }

        public Task<string> PostReturnStringAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            return ExecuteStringHttpAction(() =>
                _httpClient.PostAsync(requestUri, cancellationToken, SetupHttpRequestMessage));
        }

        public Task<string> PostReturnStringAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
        {
            return ExecuteStringHttpAction(() =>
                _httpClient.PostAsync(requestUri, completionOption, cancellationToken,
                    SetupHttpRequestMessage));
        }

        public Task<TResult> PostReturnJsonAsync<TResult>(Uri requestUri)
            where TResult : class
        {
            return ExecuteJsonHttpAction<TResult>(() =>
                _httpClient.PostAsync(requestUri, SetupHttpRequestMessage));
        }

        public Task<TResult> PostReturnJsonAsync<TResult>(Uri requestUri, CancellationToken cancellationToken)
            where TResult : class
        {
            return ExecuteJsonHttpAction<TResult>(() =>
                _httpClient.PostAsync(requestUri, cancellationToken, SetupHttpRequestMessage));
        }

        public Task<TResult> PostReturnJsonAsync<TResult>(Uri requestUri, HttpCompletionOption completionOption,
            CancellationToken cancellationToken)
            where TResult : class
        {
            return ExecuteJsonHttpAction<TResult>(() =>
                _httpClient.PostAsync(requestUri, completionOption, cancellationToken,
                    SetupHttpRequestMessage));
        }

        public Task<HttpResponseMessage> PutJsonAsync<T>(Uri requestUri, T postData)
        {
            return ExecuteHttpAction(() =>
                _httpClient.PutJsonAsync(requestUri, postData, SetupHttpRequestMessage));
        }

        public Task<HttpResponseMessage> PutJsonAsync<T>(Uri requestUri, T postData, CancellationToken cancellationToken)
        {
            return ExecuteHttpAction(() => _httpClient.PutJsonAsync(requestUri, cancellationToken, postData,
                SetupHttpRequestMessage));
        }

        public Task<HttpResponseMessage> PutJsonAsync<T>(Uri requestUri, T postData, HttpCompletionOption completionOption,
            CancellationToken cancellationToken)
        {
            return ExecuteHttpAction(() => _httpClient.PutJsonAsync(requestUri, completionOption, cancellationToken,
                postData, SetupHttpRequestMessage));
        }

        public Task<TResult> PutJsonAsync<T, TResult>(Uri requestUri, T dataObject) where TResult : class
        {
            return ExecuteJsonHttpAction<TResult>(() =>
                _httpClient.PutJsonAsync(requestUri, dataObject, SetupHttpRequestMessage));
        }

        public Task<TResult> PutJsonAsync<T, TResult>(Uri requestUri, T dataObject, CancellationToken cancellationToken) where TResult : class
        {
            return ExecuteJsonHttpAction<TResult>(() => _httpClient.PutJsonAsync(requestUri, cancellationToken,
                dataObject, SetupHttpRequestMessage));
        }

        public Task<TResult> PutJsonAsync<T, TResult>(Uri requestUri, T dataObject, HttpCompletionOption completionOption,
            CancellationToken cancellationToken) where TResult : class
        {
            return ExecuteJsonHttpAction<TResult>(() => _httpClient.PutJsonAsync(requestUri, completionOption,
                cancellationToken, dataObject, SetupHttpRequestMessage));
        }

        public Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, HttpCompletionOption completionOption,
            CancellationToken cancellationToken)
        {
            return ExecuteHttpAction(() => _httpClient.SendAsync(request, completionOption, cancellationToken, SetupHttpRequestMessage));
        }

        public IHttpAuthenticator Authenticator { get; set; }
    }
}