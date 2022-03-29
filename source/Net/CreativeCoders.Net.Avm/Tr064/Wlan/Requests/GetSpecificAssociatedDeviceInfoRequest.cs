using CreativeCoders.Net.Soap.Request;

namespace CreativeCoders.Net.Avm.Tr064.Wlan.Requests;

[SoapRequest("GetSpecificAssociatedDeviceInfo", "urn:dslforum-org:service:WLANConfiguration:1")]
public class GetSpecificAssociatedDeviceInfoRequest
{
    [SoapRequestField("NewAssociatedDeviceMACAddress")]
    public string MacAddress { get; set; }
}
