using CreativeCoders.DynamicCode.Proxying;
using CreativeCoders.Net.WebApi.Building;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Net.WebApi;

[PublicAPI]
public static class WebApiServiceCollectionExtensions
{
    public static void AddWebApiClient(this IServiceCollection services)
    {
        services.AddProxyBuilder();

        services.TryAddSingleton<IApiBuilder, ApiBuilder>();

        services.TryAddSingleton(typeof(IWebApiClientBuilder<>), typeof(WebApiClientBuilder<>));
    }

    public static void AddWebApiClient<T>(this IServiceCollection services, string baseUri)
        where T : class
    {
        services.AddWebApiClient();

        services.TryAddSingleton(sp =>
        {
            var webApiClientFactory = sp.GetRequiredService<IWebApiClientBuilder<T>>();

            return webApiClientFactory.Build(baseUri);
        });
    }
}
