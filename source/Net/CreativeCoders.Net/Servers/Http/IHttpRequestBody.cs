using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Servers.Http;

[PublicAPI]
public interface IHttpRequestBody
{
    Task<string> ReadAsStringAsync();

    Task<Stream> ReadAsStreamAsync();
}
