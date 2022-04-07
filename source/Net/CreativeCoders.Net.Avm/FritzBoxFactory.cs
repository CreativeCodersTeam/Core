using System.Net.Http;
using CreativeCoders.Core;

namespace CreativeCoders.Net.Avm;

public class FritzBoxFactory : IFritzBoxFactory
{
    private readonly IHttpClientFactory _httpClientFactory;

    public FritzBoxFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = Ensure.NotNull(httpClientFactory, nameof(httpClientFactory));
    }

    public IFritzBox Create(string name)
    {
        return new FritzBox(_httpClientFactory.CreateClient(name));
    }
}