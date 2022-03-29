using System.Text;
using System.Threading.Tasks;
using CreativeCoders.Net.XmlRpc.Definition;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Client;

[PublicAPI]
public interface IXmlRpcClient
{
    Task<XmlRpcResponse> SendRequestAsync(XmlRpcRequest request);

    Task ExecuteAsync(string methodName, params object[] parameters);

    Task<XmlRpcResponse> InvokeAsync(string methodName, params object[] parameters);

    Task<T> InvokeAsync<T>(string methodName, params object[] parameters);

    Task<XmlRpcResponse> InvokeExAsync(string methodName, object[] parameters);

    Task<T> InvokeExAsync<T>(string methodName, object[] parameters);

    Task<T> InvokeExAsync<T, TInvoke>(string methodName, object[] parameters, IMethodResultConverter resultConverter);

    string Url { get; set; }

    Encoding XmlEncoding { get; set; }

    string HttpContentType { get; set; }
}