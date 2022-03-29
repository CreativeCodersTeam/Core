using System.Net;
using CreativeCoders.Net.Soap;

namespace CreativeCoders.Net.Avm.Tr064;

public abstract class Tr064ApiBase
{
    protected Tr064ApiBase(ISoapHttpClient soapHttpClient, string fritzBoxUrl, string controlUrl,
        string userName, string password)
    {
        SoapHttpClient = soapHttpClient;
        SoapHttpClient.Url = fritzBoxUrl + "/tr064" + controlUrl;
        if (!string.IsNullOrWhiteSpace(userName))
        {
            SoapHttpClient.Credentials = new NetworkCredential(userName, password);
        }

        SoapHttpClient.AllowUntrustedCertificates = true;
    }

    protected ISoapHttpClient SoapHttpClient { get; }
}
