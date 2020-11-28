using CreativeCoders.Net.Avm.Tr064.WanPpp.Requests;
using CreativeCoders.Net.Avm.Tr064.WanPpp.Responses;
using CreativeCoders.Net.Soap;
using CreativeCoders.Net.WebRequests;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm.Tr064
{
    [PublicAPI]
    public class WanPppConnectionApi : Tr064ApiBase
    {
        public WanPppConnectionApi(string fritzBoxUrl, string userName, string password) : base(new SoapHttpClient(WebRequestFactory.Default),
            fritzBoxUrl, "/upnp/control/wanpppconn1", userName, password) { }

        public WanPppConnectionApi(ISoapHttpClient soapHttpClient, string fritzBoxUrl, string userName, string password) :
            base(soapHttpClient, fritzBoxUrl, "/upnp/control/wanpppconn1", userName, password) { }

        public GetExternalIpAddressResponse GetExternalIpAddress()
        {
            return SoapHttpClient.Invoke<GetExternalIpAddressRequest, GetExternalIpAddressResponse>(
                new GetExternalIpAddressRequest());
        }
    }
}