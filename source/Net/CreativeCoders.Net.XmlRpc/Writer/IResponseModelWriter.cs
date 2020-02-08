using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CreativeCoders.Net.XmlRpc.Writer
{
    public interface IResponseModelWriter
    {
        Task WriteAsync(Stream outputStream, XmlRpcResponse xmlRpcResponse, Encoding encoding);

        Task WriteAsync(Stream outputStream, XmlRpcResponse xmlRpcResponse);
    }
}