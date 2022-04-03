using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.Net.Avm.Tr064.WanPpp.Requests;
using CreativeCoders.Net.Avm.Tr064.WanPpp.Responses;
using CreativeCoders.Net.Soap;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm.Tr064;

[PublicAPI]
public class WanPppConnectionApi : Tr064ApiBase
{
    public WanPppConnectionApi(IHttpClientFactory httpClientFactory, string fritzBoxUrl, string userName, string password) : base(
        new SoapHttpClient(httpClientFactory),
        fritzBoxUrl, "/upnp/control/wanpppconn1", userName, password) { }

    public WanPppConnectionApi(ISoapHttpClient soapHttpClient, string fritzBoxUrl, string userName,
        string password) :
        base(soapHttpClient, fritzBoxUrl, "/upnp/control/wanpppconn1", userName, password) { }

    public async Task<GetExternalIpAddressResponse> GetExternalIpAddressAsync()
    {
        return await SoapHttpClient.InvokeAsync<GetExternalIpAddressRequest, GetExternalIpAddressResponse>(
            new GetExternalIpAddressRequest())
            .ConfigureAwait(false);
    }
}
