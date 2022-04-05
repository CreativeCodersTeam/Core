using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.Net.Avm.Tr064.Hosts.Requests;
using CreativeCoders.Net.Avm.Tr064.Hosts.Responses;
using CreativeCoders.Net.Soap;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm.Tr064;

[PublicAPI]
public class HostsApi : Tr064ApiBase
{
    public HostsApi(HttpClient httpClient, string fritzBoxUrl) : this(
        new SoapHttpClient(httpClient), fritzBoxUrl) { }

    public HostsApi(ISoapHttpClient soapHttpClient, string fritzBoxUrl) :
        base(soapHttpClient, fritzBoxUrl, "/upnp/control/hosts") { }


    public async Task<GetHostNumberOfEntriesResponse> GetHostNumberOfEntriesAsync()
    {
        return await SoapHttpClient
            .InvokeAsync<GetHostNumberOfEntriesRequest, GetHostNumberOfEntriesResponse>(
                new GetHostNumberOfEntriesRequest())
            .ConfigureAwait(false);
    }

    public async Task<GetSpecificHostEntryResponse> GetSpecificHostEntryAsync(string macAddress)
    {
        return await SoapHttpClient
            .InvokeAsync<GetSpecificHostEntryRequest, GetSpecificHostEntryResponse>(
                new GetSpecificHostEntryRequest {MacAddress = macAddress})
            .ConfigureAwait(false);
    }

    public async Task<GetGenericHostEntryResponse> GetGenericHostEntryAsync(int index)
    {
        return await SoapHttpClient
            .InvokeAsync<GetGenericHostEntryRequest, GetGenericHostEntryResponse>(
                new GetGenericHostEntryRequest {Index = index})
            .ConfigureAwait(false);
    }
}
