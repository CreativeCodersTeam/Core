namespace CreativeCoders.Net.Servers.Http;

public interface IHttpRequest
{
    IHttpRequestBody Body { get; }

    string ContentType { get; }

    string HttpMethod { get; }
}