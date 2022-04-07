using System.Threading.Tasks;

namespace CreativeCoders.Net.Servers.Http;

public interface IHttpRequestHandler
{
    Task ProcessAsync(IHttpRequest request, IHttpResponse response);
}
