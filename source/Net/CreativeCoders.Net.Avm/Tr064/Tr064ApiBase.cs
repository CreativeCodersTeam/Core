using System;
using System.Net;
using CreativeCoders.Net.Soap;

namespace CreativeCoders.Net.Avm.Tr064;

public abstract class Tr064ApiBase
{
    protected Tr064ApiBase(ISoapHttpClient soapHttpClient, string controlUrl)
    {
        SoapHttpClient = soapHttpClient;
        Url = new Uri("/tr064" + controlUrl, UriKind.Relative);
    }

    protected ISoapHttpClient SoapHttpClient { get; }

    protected Uri Url { get; }
}
