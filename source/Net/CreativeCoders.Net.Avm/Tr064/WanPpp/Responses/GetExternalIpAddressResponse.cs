using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Net.Soap.Response;

namespace CreativeCoders.Net.Avm.Tr064.WanPpp.Responses;

[SoapResponse("GetExternalIPAddressResponse", "urn:dslforum-org:service:WANPPPConnection:1")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class GetExternalIpAddressResponse
{
    [SoapResponseField("NewExternalIPAddress")]
    public string IpAddress { get; set; }
}
