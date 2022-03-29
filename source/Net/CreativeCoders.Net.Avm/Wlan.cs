using CreativeCoders.Net.Avm.Tr064;

namespace CreativeCoders.Net.Avm;

public class Wlan
{
    private readonly WlanApi _wlanApi;

    public Wlan(string url, string userName, string password)
    {
        _wlanApi = new WlanApi(url, userName, password);
    }

    public WlanDeviceInfo GetWlanDeviceInfo(string macAddress)
    {
        var response = _wlanApi.GetSpecificAssociatedDeviceInfo(macAddress);

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
