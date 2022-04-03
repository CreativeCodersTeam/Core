using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.Net.Avm.Tr064;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm;

[PublicAPI]
public class WanPppConnection
{
    private readonly WanPppConnectionApi _wanPppApi;

    public WanPppConnection(IHttpClientFactory httpClientFactory, string url, string userName, string password)
    {
        _wanPppApi = new WanPppConnectionApi(httpClientFactory, url, userName, password);
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
