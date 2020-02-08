using System.IO;
using System.Threading.Tasks;

namespace CreativeCoders.Net.Servers.Http
{
    public interface IHttpResponseBody
    {
        Task WriteAsync(string content);

        Task FlushAsync();

        Stream GetStream();
    }
}