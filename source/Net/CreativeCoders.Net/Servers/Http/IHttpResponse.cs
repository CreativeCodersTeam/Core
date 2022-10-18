using JetBrains.Annotations;

namespace CreativeCoders.Net.Servers.Http;

[PublicAPI]
public interface IHttpResponse
{
    string ContentType { get; set; }

    long? ContentLength { get; set; }

    int StatusCode { get; set; }

    IHttpResponseBody Body { get; }
}
