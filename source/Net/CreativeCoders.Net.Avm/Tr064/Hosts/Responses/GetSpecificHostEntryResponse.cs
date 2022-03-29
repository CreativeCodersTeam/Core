using CreativeCoders.Net.Soap.Response;

namespace CreativeCoders.Net.Avm.Tr064.Hosts.Responses;

[SoapResponse("GetSpecificHostEntryResponse", "urn:dslforum-org:service:Hosts:1")]
public class GetSpecificHostEntryResponse
{
    [SoapResponseField("NewIPAddress")]
    public string IpAddress { get; set; }

    [SoapResponseField("NewAddressSource")]
    public string AddressSource { get; set; }

    [SoapResponseField("NewLeaseTimeRemaining")]
    public int LeaseTimeRemaining { get; set; }

    [SoapResponseField("NewInterfaceType")]
    public string InterfaceType { get; set; }

    [SoapResponseField("NewActive")]
    public int Active { get; set; }

    [SoapResponseField("NewHostName")]
    public string HostName { get; set; }
}