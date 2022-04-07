using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Net.Soap.Response;

namespace CreativeCoders.Net.Avm.Tr064.Hosts.Responses;

[SoapResponse("GetHostNumberOfEntriesResponse", "urn:dslforum-org:service:Hosts:1")]
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class GetHostNumberOfEntriesResponse
{
    [SoapResponseField("NewHostNumberOfEntries")]
    public int HostNumberOfEntries { get; set; }
}
