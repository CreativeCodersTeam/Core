using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Net.XmlRpc;

public static class XmlRpcServiceCollectionExtensions
{
    public static void AddXmlRpc(this IServiceCollection services)
    {
        services.AddHttpClient();

        //services.add
    }
}
