using Microsoft.AspNetCore.Http;

namespace CreativeCoders.Net.Servers.Http.AspNetCore;

public class AspNetCoreHttpResponseWrapper : IHttpResponse
{
    private readonly HttpResponse _httpResponse;

    public AspNetCoreHttpResponseWrapper(HttpResponse httpResponse)
    {
        _httpResponse = httpResponse;
        Body = new StreamResponseBody(httpResponse.Body);
    }

    public string ContentType
    {
        get => _httpResponse.ContentType;
        set => _httpResponse.ContentType = value;
    }

    public long? ContentLength
    {
        get => _httpResponse.ContentLength;
        set => _httpResponse.ContentLength = value;
    }

    public int StatusCode
    {
        get => _httpResponse.StatusCode;
        set => _httpResponse.StatusCode = value;
    }

    public IHttpResponseBody Body { get; }
}
