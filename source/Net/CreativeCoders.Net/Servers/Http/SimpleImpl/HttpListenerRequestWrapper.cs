using System.Net;

namespace CreativeCoders.Net.Servers.Http.SimpleImpl
{
    internal class HttpListenerRequestWrapper : IHttpRequest
    {
        private readonly HttpListenerRequest _request;

        public HttpListenerRequestWrapper(HttpListenerRequest request)
        {
            _request = request;
            Body = new StreamRequestBody(request.InputStream);
        }

        public IHttpRequestBody Body { get; }

        public string ContentType => _request.ContentType;

        public string HttpMethod => _request.HttpMethod;
    }
}