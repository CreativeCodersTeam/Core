using System.Net;
using CreativeCoders.Net.Soap;

namespace CreativeCoders.Net.Avm.Tr064;

public abstract class Tr064ApiBase
{
    protected Tr064ApiBase(ISoapHttpClient soapHttpClient, string fritzBoxUrl, string controlUrl)
    {
        SoapHttpClient = soapHttpClient;
        SoapHttpClient.Url = fritzBoxUrl + "/tr064" + controlUrl;
    }

    protected ISoapHttpClient SoapHttpClient { get; }
}
