using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.Net.WebApi.Definition;
using CreativeCoders.Net.WebApi.Serialization;
using CreativeCoders.Net.WebApi.Specification;

namespace CreativeCoders.Net.WebApi.Execution.Requests
{
    public abstract class ApiRequestHandlerBase : IApiRequestHandler
    {
        private readonly HttpClient _httpClient;

        protected ApiRequestHandlerBase(ApiMethodReturnType methodReturnType, HttpClient httpClient)
        {
            _httpClient = httpClient;
            MethodReturnType = methodReturnType;
        }

        public ApiMethodReturnType MethodReturnType { get; }

        public abstract object SendRequest(RequestData requestData);

        private static HttpMethod GetHttpMethod(HttpRequestMethod requestMethod)
        {
            return requestMethod switch
            {
                HttpRequestMethod.Get => HttpMethod.Get,
                HttpRequestMethod.Post => HttpMethod.Post,
                HttpRequestMethod.Put => HttpMethod.Put,
                HttpRequestMethod.Delete => HttpMethod.Delete,
                _ => throw new ArgumentOutOfRangeException(nameof(requestMethod), requestMethod, null)
            };
        }

        private static HttpContent GetBodyContent(RequestData requestData)
        {
            var bodyValue = requestData.GetBodyValue();

            if (bodyValue == null)
            {
                return null;
            }

            return bodyValue switch
            {
                string bodyString => new StringContent(bodyString),
                Stream bodyStream => new StreamContent(bodyStream),
                HttpContent bodyContent => bodyContent,
                byte[] bodyBytes => new ByteArrayContent(bodyBytes),
                _ => new StringContent(requestData.DefaultDataFormatter.GetSerializer().Serialize(bodyValue),
                    Encoding.UTF8, requestData.DefaultDataFormatter.ContentMediaType)
            };
        }

        private static HttpRequestMessage CreateRequestMessage(RequestData requestData)
        {
            var request = new HttpRequestMessage(GetHttpMethod(requestData.RequestMethod), requestData.RequestUri)
            {
                Content = GetBodyContent(requestData)
            };

            requestData.Headers.ForEach(h => request.Headers.Add(h.Name, h.Value));

            return request;
        }

        protected Task<HttpResponseMessage> RequestResponseMessageAsync(RequestData requestData)
        {
            var httpRequest = CreateRequestMessage(requestData);

            return _httpClient.SendAsync(httpRequest, requestData.CompletionOption,
                requestData.CancellationToken);
        }

        protected static IDataDeserializer GetResponseDeserializer(RequestData requestData)
        {
            return requestData.ResponseDeserializer ?? requestData.DefaultDataFormatter.GetDeserializer();
        }
    }
}