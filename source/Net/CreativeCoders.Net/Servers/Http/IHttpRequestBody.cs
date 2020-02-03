using System.IO;
using System.Threading.Tasks;

namespace CreativeCoders.Net.Servers.Http
{
    public interface IHttpRequestBody
    {
        Task<string> ReadAsStringAsync();

        Task<Stream> ReadAsStreamAsync();
    }
}