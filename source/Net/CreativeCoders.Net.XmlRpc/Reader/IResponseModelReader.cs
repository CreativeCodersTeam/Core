using System.IO;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Reader
{
    [PublicAPI]
    public interface IResponseModelReader
    {
        Task<XmlRpcResponse> ReadAsync(Stream inputStream, bool isMultiCallResponse);
    }
}