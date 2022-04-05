using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Net.Http;

public static class NamedHttpClientFactoryOptionsServiceCollectionExtensions
{
    public static void AddNamedHttpClientOptions(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.AddSingleton<INamedHttpClientFactoryOptionsStore, NamedHttpClientFactoryOptionsStore>();

        services.ConfigureOptions<NamedHttpClientFactoryOptions>();
    }
}
