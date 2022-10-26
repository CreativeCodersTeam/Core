using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.DynamicCode.Proxying;

public static class ProxyBuilderServiceCollectionExtensions
{
    public static void AddProxyBuilder(this IServiceCollection services)
    {
        services.TryAddTransient<IProxyGenerator, ProxyGenerator>();

        services.TryAddTransient(typeof(IProxyBuilder<>), typeof(ProxyBuilder<>));

        services.TryAddSingleton<IProxyBuilderFactory, ProxyBuilderFactory>();
    }
}
