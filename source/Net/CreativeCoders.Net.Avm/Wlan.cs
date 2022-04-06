﻿using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.Net.Avm.Tr064;

namespace CreativeCoders.Net.Avm;

public class Wlan
{
    private readonly WlanApi _wlanApi;

    public Wlan(HttpClient httpClient)
    {
        _wlanApi = new WlanApi(httpClient);
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
