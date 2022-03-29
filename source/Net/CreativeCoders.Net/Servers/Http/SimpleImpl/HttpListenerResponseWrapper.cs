using System;
using System.Net;

namespace CreativeCoders.Net.Servers.Http.SimpleImpl;

internal class HttpListenerResponseWrapper : IHttpResponse
{
    private readonly HttpListenerResponse _response;

    public HttpListenerResponseWrapper(HttpListenerResponse response)
    {
        _response = response;
        Body = new StreamResponseBody(_response.OutputStream);
    }

    public string ContentType
    {
        get => _response.ContentType;
        set => _response.ContentType = value;
    }

    public long? ContentLength
    {
        get => _response.ContentLength64;
        set => _response.ContentLength64 = value ?? throw new NotSupportedException();
    }

    public int StatusCode
    {
        get => _response.StatusCode;
        set => _response.StatusCode = value;
    }

    public IHttpResponseBody Body { get; }
}