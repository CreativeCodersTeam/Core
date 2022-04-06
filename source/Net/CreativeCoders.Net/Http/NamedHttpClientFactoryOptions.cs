using CreativeCoders.Core;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace CreativeCoders.Net.Http;

public class NamedHttpClientFactoryOptions : IConfigureNamedOptions<HttpClientFactoryOptions>
{
    private readonly IHttpClientSettings _optionsStore;

    public NamedHttpClientFactoryOptions(IHttpClientSettings optionsStore)
    {
        _optionsStore = Ensure.NotNull(optionsStore, nameof(optionsStore));
    }

    public void Configure(HttpClientFactoryOptions options) { }

    public void Configure(string name, HttpClientFactoryOptions options)
    {
        var configure = _optionsStore.Get(name);

        configure?.Invoke(options);
    }
}
