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
    public WanPppConnectionApi(HttpClient httpClient, string fritzBoxUrl) : base(
        new SoapHttpClient(httpClient),
        fritzBoxUrl, "/upnp/control/wanpppconn1") { }

    public WanPppConnectionApi(ISoapHttpClient soapHttpClient, string fritzBoxUrl) :
        base(soapHttpClient, fritzBoxUrl, "/upnp/control/wanpppconn1") { }

    public async Task<GetExternalIpAddressResponse> GetExternalIpAddressAsync()
    {
        return await SoapHttpClient.InvokeAsync<GetExternalIpAddressRequest, GetExternalIpAddressResponse>(
            new GetExternalIpAddressRequest())
            .ConfigureAwait(false);
    }
}
