using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.Net.Avm.Tr064;

namespace CreativeCoders.Net.Avm.Wan;

internal class WanPppConnectionImpl : IWanPppConnection
{
    private readonly WanPppConnectionApi _wanPppApi;

    public WanPppConnectionImpl(HttpClient httpClient)
    {
        _wanPppApi = new WanPppConnectionApi(httpClient);
    }

    public async Task<string> GetExternalIpAddressAsync()
    {
        var response = await _wanPppApi.GetExternalIpAddressAsync()
            .ConfigureAwait(false);

        return response?.IpAddress ?? string.Empty;
    }

    public async Task<IPAddress> GetExternalIpAddressExAsync()
    {
        var ipAddress = await GetExternalIpAddressAsync()
            .ConfigureAwait(false);

        return IPAddress.TryParse(ipAddress, out var ip) ? ip : IPAddress.None;
    }
}
