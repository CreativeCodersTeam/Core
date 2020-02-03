using Microsoft.AspNetCore.Http;

namespace CreativeCoders.Net.Servers.Http.AspNetCore
{
    public class AspNetCoreHttpRequestWrapper : IHttpRequest
    {
        private readonly HttpRequest _httpRequest;

        public AspNetCoreHttpRequestWrapper(HttpRequest httpRequest)
        {
            _httpRequest = httpRequest;
            Body = new StreamRequestBody(httpRequest.Body);
        }

        public IHttpRequestBody Body { get; }

        public string ContentType => _httpRequest.ContentType;

        public string HttpMethod => _httpRequest.Method;
    }
}