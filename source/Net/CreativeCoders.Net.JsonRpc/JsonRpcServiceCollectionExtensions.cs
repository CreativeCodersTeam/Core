using System.Diagnostics.CodeAnalysis;
using CreativeCoders.DynamicCode.Proxying;
using CreativeCoders.Net.JsonRpc;
using CreativeCoders.Net.JsonRpc.ApiBuilder;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

[PublicAPI]
[ExcludeFromCodeCoverage]
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
