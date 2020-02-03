using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CreativeCoders.Net.XmlRpc.Writer
{
    public interface IRequestModelWriter
    {
        Task WriteAsync(Stream outputStream, XmlRpcRequest xmlRpcRequest, Encoding encoding);

        Task WriteAsync(Stream outputStream, XmlRpcRequest xmlRpcRequest);
    }
}