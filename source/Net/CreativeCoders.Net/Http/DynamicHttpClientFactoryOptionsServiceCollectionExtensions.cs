using CreativeCoders.Core;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Net.Http;

public static class DynamicHttpClientFactoryOptionsServiceCollectionExtensions
{
    public static void AddDynamicHttpClient(this IServiceCollection services)
    {
        Ensure.NotNull(services, nameof(services));

        services.AddHttpClient();

        services.AddSingleton<IHttpClientSettings, HttpClientSettings>();

        services.ConfigureOptions<DynamicHttpClientFactoryOptions>();
    }
}
