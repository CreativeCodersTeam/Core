using System.Collections.Generic;
using System.Threading.Tasks;

namespace CreativeCoders.Net.Servers.Http;

public interface IHttpServer
{
    Task StartAsync();

    Task StopAsync();

    void RegisterRequestHandler(IHttpRequestHandler requestHandler);

    IList<string> Urls { get; }
}
