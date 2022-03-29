using CreativeCoders.Net.Soap.Request;

namespace CreativeCoders.Net.Avm.Tr064.Hosts.Requests;

[SoapRequest("GetGenericHostEntry", "urn:dslforum-org:service:Hosts:1")]
public class GetGenericHostEntryRequest
{
    [SoapRequestField("NewIndex")] public int Index { get; set; }
}
