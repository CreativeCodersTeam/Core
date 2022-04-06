using System;
using System.Net.Http;

namespace CreativeCoders.Net.Http;

public interface IHttpClientSetup
{
    IHttpClientSetup ConfigureClient(Action<HttpClient> configureClient);

    IHttpClientSetup ConfigureClientHandler(Func<HttpClientHandler> configureClientHandler);
}