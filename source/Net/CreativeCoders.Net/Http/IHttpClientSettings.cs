using System;
using Microsoft.Extensions.Http;

namespace CreativeCoders.Net.Http;

#nullable enable
public interface IHttpClientSettings
{
    void Add(string name, Action<HttpClientFactoryOptions> configure);

    IHttpClientSetup Add(string name);

    Action<HttpClientFactoryOptions>? Get(string name);
}
