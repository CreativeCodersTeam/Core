using System.Text;

namespace CreativeCoders.Net.XmlRpc.Client;

public interface IRequestBuilder
{
    XmlRpcRequest Build(string methodName, params object[] parameters);

    Encoding XmlEncoding { get; set; }
}