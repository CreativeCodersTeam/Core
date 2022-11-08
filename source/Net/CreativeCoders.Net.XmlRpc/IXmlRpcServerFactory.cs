using CreativeCoders.Net.Servers.Http;
using CreativeCoders.Net.XmlRpc.Server;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc;

[PublicAPI]
public interface IXmlRpcServerFactory
{
    IXmlRpcServer CreateServer();

    IXmlRpcServer CreateServer(IHttpServer httpServer);
}
