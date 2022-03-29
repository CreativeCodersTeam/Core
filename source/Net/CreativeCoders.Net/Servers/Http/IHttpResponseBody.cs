using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Servers.Http;

[PublicAPI]
public interface IHttpResponseBody
{
    Task WriteAsync(string content);

    Task FlushAsync();

    Stream GetStream();
}