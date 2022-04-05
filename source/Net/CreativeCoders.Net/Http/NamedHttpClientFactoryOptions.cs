using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;

namespace CreativeCoders.Net.Http;

public class NamedHttpClientFactoryOptions : IConfigureNamedOptions<HttpClientFactoryOptions>
{
    private readonly INamedHttpClientFactoryOptionsStore _optionsStore;

    public NamedHttpClientFactoryOptions(INamedHttpClientFactoryOptionsStore optionsStore)
    {
        _optionsStore = optionsStore;
    }

    public void Configure(HttpClientFactoryOptions options) { }

    public void Configure(string name, HttpClientFactoryOptions options)
    {
        var configure = _optionsStore.Get(name);

        configure?.Invoke(options);
    }
}
