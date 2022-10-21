using CreativeCoders.Net.WebApi.Building;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Net.WebApi;

public static class ServiceCollectionExtensions
{
    public static void AddWebApiClient(this IServiceCollection services)
    {
        services.TryAddSingleton<IApiBuilder, ApiBuilder>();

        services.TryAddSingleton<IWebApiClientFactory, WebApiClientFactory>();
    }

    public static void AddWebApiClient<T>(this IServiceCollection services, string baseUri)
        where T : class
    {
        services.AddWebApiClient();

        services.TryAddSingleton<T>(sp =>
        {
            var webApiClientFactory = sp.GetRequiredService<IWebApiClientFactory>();

            return webApiClientFactory.CreateBuilder<T>().Build(baseUri);
        });
    }
}
