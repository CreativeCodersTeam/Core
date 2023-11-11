using System;
using System.Collections.Concurrent;
using CreativeCoders.Core;
using Microsoft.Extensions.Http;

namespace CreativeCoders.Net.Http;

#nullable enable
public class HttpClientSettings : IHttpClientSettings
{
    private readonly ConcurrentDictionary<string, Action<HttpClientFactoryOptions>> _options = new();

    public void Add(string name, Action<HttpClientFactoryOptions> configure)
    {
        Ensure.NotNull(name, nameof(name));
        Ensure.NotNull(configure, nameof(configure));

        _options[name] = configure;
    }

    public IHttpClientSetup Add(string name)
    {
        Ensure.NotNull(name, nameof(name));

        var clientSetup = new HttpClientSetup();

        _options[name] = options => clientSetup.InvokeSetup(options);

        return clientSetup;
    }

    public Action<HttpClientFactoryOptions>? Get(string name)
    {
        Ensure.NotNull(name, nameof(name));

        return _options.TryGetValue(name, out var value)
            ? value
            : null;
    }
}
