using System;
using System.Net.Http;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Http;

[PublicAPI]
public interface IHttpClientSetup
{
    IHttpClientSetup ConfigureClient(Action<HttpClient> configureClient);

    IHttpClientSetup ConfigureClientHandler(Func<HttpClientHandler> configureClientHandler);
}
