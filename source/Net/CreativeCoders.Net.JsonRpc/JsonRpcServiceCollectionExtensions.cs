using CreativeCoders.DynamicCode.Proxying;
using CreativeCoders.Net.JsonRpc.ApiBuilder;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Net.JsonRpc;

[PublicAPI]
public static class JsonRpcServiceCollectionExtensions
{
    public static IServiceCollection AddJsonRpcClient(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.TryAddTransient<IJsonRpcClient, JsonRpcClient>();

        services.TryAddSingleton<IJsonRpcClientFactory, JsonRpcClientFactory>();

        services.AddProxyBuilder();

        services.TryAddSingleton(typeof(IJsonRpcApiBuilder<>), typeof(JsonRpcApiBuilder<>));

        return services;
    }
}
