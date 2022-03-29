using CreativeCoders.Net.Soap.Response;

namespace CreativeCoders.Net.Avm.Tr064.Wlan.Responses;

[SoapResponse("GetSpecificAssociatedDeviceInfoResponse", "urn:dslforum-org:service:WLANConfiguration:1")]
public class GetSpecificAssociatedDeviceInfoResponse
{
    [SoapResponseField("NewAssociatedDeviceIPAddress")]
    public string IpAddress { get; set; }

    [SoapResponseField("NewAssociatedDeviceAuthState")]
    public string DeviceAuthState { get; set; }

    [SoapResponseField("NewX_AVM-DE_Speed")]
    public int Speed { get; set; }

    [SoapResponseField("NewX_AVM-DE_SignalStrength")]
    public int SignalStrength { get; set; }
}