using System;
using CreativeCoders.Core;
using CreativeCoders.Net.Servers.Http;
using CreativeCoders.Net.XmlRpc.Server;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Net.XmlRpc;

public class XmlRpcServerFactory : IXmlRpcServerFactory
{
    private readonly IServiceProvider _serviceProvider;

    public XmlRpcServerFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = Ensure.NotNull(serviceProvider, nameof(serviceProvider));
    }

    public IXmlRpcServer CreateServer()
    {
        return new XmlRpcServer(_serviceProvider.GetRequiredService<IHttpServer>(), false);
    }

    public IXmlRpcServer CreateServer(IHttpServer httpServer)
    {
        return new XmlRpcServer(httpServer, true);
    }
}
