using System;
using Microsoft.Extensions.Http;

namespace CreativeCoders.Net.Http;

public interface INamedHttpClientFactoryOptionsStore
{
    void Add(string name, Action<HttpClientFactoryOptions> configure);

    Action<HttpClientFactoryOptions> Get(string name);
}
