using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.Net.Avm.Tr064;

namespace CreativeCoders.Net.Avm;

public class Wlan
{
    private readonly WlanApi _wlanApi;

    public Wlan(IHttpClientFactory httpClientFactory, string url, string userName, string password)
    {
        _wlanApi = new WlanApi(httpClientFactory, url, userName, password);
    }

    public async Task<WlanDeviceInfo> GetWlanDeviceInfo(string macAddress)
    {
        var response = await _wlanApi.GetSpecificAssociatedDeviceInfoAsync(macAddress)
            .ConfigureAwait(false);

        return new WlanDeviceInfo
        {
            MacAddress = macAddress,
            IpAddress = response.IpAddress,
            DeviceAuthState = response.DeviceAuthState,
            SignalStrength = response.SignalStrength,
            Speed = response.Speed
        };
    }
}
