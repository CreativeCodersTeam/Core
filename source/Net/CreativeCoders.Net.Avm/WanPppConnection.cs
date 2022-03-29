using System.Net;
using CreativeCoders.Net.Avm.Tr064;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm;

[PublicAPI]
public class WanPppConnection
{
    private readonly WanPppConnectionApi _wanPppApi;

    public WanPppConnection(string url, string userName, string password)
    {
        _wanPppApi = new WanPppConnectionApi(url, userName, password);
    }

    public string GetExternalIpAddress()
    {
        var response = _wanPppApi.GetExternalIpAddress();
        return response?.IpAddress ?? string.Empty;
    }

    public IPAddress GetExternalIpAddressEx()
    {
        var ipAddress = GetExternalIpAddress();
        return IPAddress.TryParse(ipAddress, out var ip) ? ip : IPAddress.None;
    }
}
