using CreativeCoders.DynamicCode.Proxying;
using CreativeCoders.Net.XmlRpc.Proxy;
using CreativeCoders.Net.XmlRpc.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Net.XmlRpc;

public static class XmlRpcServiceCollectionExtensions
{
    public static void AddXmlRpc(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.AddProxyBuilder();

        services.TryAddSingleton(typeof(IXmlRpcProxyBuilder<>), typeof(XmlRpcProxyBuilder<>));

        services.TryAddTransient<IXmlRpcServer, XmlRpcServer>();

        services.TryAddSingleton<IXmlRpcServerFactory, XmlRpcServerFactory>();
    }
}
