using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Http;

namespace CreativeCoders.Net.Http;

public class NamedHttpClientFactoryOptionsStore : INamedHttpClientFactoryOptionsStore
{
    private readonly IDictionary<string, Action<HttpClientFactoryOptions>> _options;

    public NamedHttpClientFactoryOptionsStore()
    {
        _options = new ConcurrentDictionary<string, Action<HttpClientFactoryOptions>>();
    }

    public void Add(string name, Action<HttpClientFactoryOptions> configure)
    {
        _options[name] = configure;
    }

    public Action<HttpClientFactoryOptions> Get(string name)
    {
        return _options[name];
    }
}
