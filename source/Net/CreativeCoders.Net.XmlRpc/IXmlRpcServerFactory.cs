using CreativeCoders.Net.Servers.Http;
using CreativeCoders.Net.XmlRpc.Server;

namespace CreativeCoders.Net.XmlRpc;

public interface IXmlRpcServerFactory
{
    IXmlRpcServer CreateServer();

    IXmlRpcServer CreateServer(IHttpServer httpServer);
}
