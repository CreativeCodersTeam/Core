using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.Net.Avm.Tr064;

namespace CreativeCoders.Net.Avm.Wlan;

internal class WlanImpl : IWlan
{
    private readonly WlanApi _wlanApi;

    public WlanImpl(HttpClient httpClient)
    {
        _wlanApi = new WlanApi(httpClient);
    }

    public async Task<WlanDeviceInfo> GetWlanDeviceInfoAsync(string macAddress)
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
