using System.Text;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Proxy
{
    [PublicAPI]
    public interface IXmlRpcProxyBuilder<out T>
        where T : class
    {
        IXmlRpcProxyBuilder<T> ForUrl(string url);

        IXmlRpcProxyBuilder<T> UseEncoding(Encoding encoding);
        
        IXmlRpcProxyBuilder<T> UseEncoding(string encodingName);

        IXmlRpcProxyBuilder<T> WithContentType(string contentType);

        T Build();
    }
}