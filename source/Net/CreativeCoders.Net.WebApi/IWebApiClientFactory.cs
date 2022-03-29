using JetBrains.Annotations;

namespace CreativeCoders.Net.WebApi;

[PublicAPI]
public interface IWebApiClientFactory
{
    IWebApiClientBuilder<T> CreateBuilder<T>()
        where T : class;
}