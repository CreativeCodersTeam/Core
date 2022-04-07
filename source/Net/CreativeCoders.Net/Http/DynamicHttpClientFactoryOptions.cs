using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Core;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace CreativeCoders.Net.Http;

public class DynamicHttpClientFactoryOptions : IConfigureNamedOptions<HttpClientFactoryOptions>
{
    private readonly IHttpClientSettings _optionsStore;

    public DynamicHttpClientFactoryOptions(IHttpClientSettings optionsStore)
    {
        _optionsStore = Ensure.NotNull(optionsStore, nameof(optionsStore));
    }

    [ExcludeFromCodeCoverage]
    public void Configure(HttpClientFactoryOptions options) { }

    public void Configure(string name, HttpClientFactoryOptions options)
    {
        var configure = _optionsStore.Get(name);

        configure?.Invoke(options);
    }
}
