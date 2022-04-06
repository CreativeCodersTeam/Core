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
    public WanPppConnectionApi(HttpClient httpClient) : base(
        new SoapHttpClient(httpClient), "/upnp/control/wanpppconn1") { }

    public WanPppConnectionApi(ISoapHttpClient soapHttpClient) :
        base(soapHttpClient, "/upnp/control/wanpppconn1") { }

    public async Task<GetExternalIpAddressResponse> GetExternalIpAddressAsync()
    {
        return await SoapHttpClient.InvokeAsync<GetExternalIpAddressRequest, GetExternalIpAddressResponse>(Url,
            new GetExternalIpAddressRequest())
            .ConfigureAwait(false);
    }
}
