using System;
using System.Net.Http;
using System.Text;
using CreativeCoders.Core;
using CreativeCoders.DynamicCode.Proxying;
using CreativeCoders.Net.XmlRpc.Client;
using CreativeCoders.Net.XmlRpc.Proxy.Analyzing;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Proxy;

[PublicAPI]
public class XmlRpcProxyBuilder<T> : IXmlRpcProxyBuilder<T>
    where T : class
{
    private readonly IProxyBuilder<T> _proxyBuilder;

    private readonly IHttpClientFactory _httpClientFactory;

    private string _url;

    private Encoding _encoding;

    private string _contentType;

    public XmlRpcProxyBuilder(IProxyBuilder<T> proxyBuilder, IHttpClientFactory httpClientFactory)
    {
        Ensure.IsNotNull(proxyBuilder, nameof(proxyBuilder));
        Ensure.IsNotNull(httpClientFactory, nameof(httpClientFactory));

        if (!typeof(T).IsInterface)
        {
            throw new ArgumentException($"Generic type '{typeof(T).Name}' must be an interface");
        }

        _proxyBuilder = proxyBuilder;
        _httpClientFactory = httpClientFactory;

        SetDefaultSettings();
    }

    private void SetDefaultSettings()
    {
        _url = null;
        _encoding = Encoding.UTF8;
        _contentType = ContentMediaTypes.Text.Xml;
    }

    public IXmlRpcProxyBuilder<T> ForUrl(string url)
    {
        _url = url;
        return this;
    }

    public IXmlRpcProxyBuilder<T> UseEncoding(Encoding encoding)
    {
        _encoding = encoding;
        return this;
    }

    public IXmlRpcProxyBuilder<T> UseEncoding(string encodingName)
    {
        return UseEncoding(Encoding.GetEncoding(encodingName));
    }

    public IXmlRpcProxyBuilder<T> WithContentType(string contentType)
    {
        _contentType = contentType;
        return this;
    }

    public T Build()
    {
        try
        {
            return _proxyBuilder.Build(
                new XmlRpcProxyInterceptor<T>(
                    new XmlRpcClient(_httpClientFactory.CreateClient())
                    {
                        Url = _url,
                        XmlEncoding = _encoding,
                        HttpContentType = _contentType
                    },
                    new ApiAnalyzer<T>().Analyze())
            );
        }
        finally
        {
            SetDefaultSettings();
        }
    }
}
