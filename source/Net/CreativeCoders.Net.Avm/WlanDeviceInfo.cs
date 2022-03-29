using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm;

[PublicAPI]
public class WlanDeviceInfo
{
    public string MacAddress { get; set; }

    public string IpAddress { get; set; }

    public string DeviceAuthState { get; set; }

    public int Speed { get; set; }

    public int SignalStrength { get; set; }
}
