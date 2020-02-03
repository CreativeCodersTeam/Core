using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Net.Http;
using CreativeCoders.Net.WebApi.Definition;
using CreativeCoders.Net.WebApi.Serialization;
using CreativeCoders.Net.WebApi.Specification;

namespace CreativeCoders.Net.WebApi.Execution.Requests
{
    public abstract class ApiRequestHandlerBase : IApiRequestHandler
    {
        private readonly IHttpClient _httpClient;

        protected ApiRequestHandlerBase(ApiMethodReturnType methodReturnType, IHttpClient httpClient)
        {
            _httpClient = httpClient;
            MethodReturnType = methodReturnType;
        }

        public ApiMethodReturnType MethodReturnType { get; }

        public abstract object SendRequest(RequestData requestData);

        private HttpMethod GetHttpMethod(HttpRequestMethod requestMethod)
        {
            switch (requestMethod)
            {
                case HttpRequestMethod.Get:
                    return HttpMethod.Get;
                case HttpRequestMethod.Post:
                    return HttpMethod.Post;
                case HttpRequestMethod.Put:
                    return HttpMethod.Put;
                case HttpRequestMethod.Delete:
                    return HttpMethod.Delete;
                default:
                    throw new ArgumentOutOfRangeException(nameof(requestMethod), requestMethod, null);
            }
        }

        private static HttpContent GetBodyContent(RequestData requestData)
        {
            var bodyValue = requestData.GetBodyValue();

            if (bodyValue == null)
            {
                return null;
            }

            switch (bodyValue)
            {
                case string bodyString:
                    return new StringContent(bodyString);
                case Stream bodyStream:
                    return new StreamContent(bodyStream);
                case HttpContent bodyContent:
                    return bodyContent;
                case byte[] bodyBytes:
                    return new ByteArrayContent(bodyBytes);
                default:
                    return new StringContent(requestData.DefaultDataFormatter.GetSerializer().Serialize(bodyValue),
                        Encoding.UTF8, requestData.DefaultDataFormatter.ContentMediaType);
            }
        }

        private HttpRequestMessage CreateRequestMessage(RequestData requestData)
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

            return _httpClient.SendRequestAsync(httpRequest, requestData.CompletionOption,
                requestData.CancellationToken);
        }

        protected static IDataDeserializer GetResponseDeserializer(RequestData requestData)
        {
            return requestData.ResponseDeserializer ?? requestData.DefaultDataFormatter.GetDeserializer();
        }
    }
}