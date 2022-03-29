using CreativeCoders.Net.Avm.Tr064.Hosts.Requests;
using CreativeCoders.Net.Avm.Tr064.Hosts.Responses;
using CreativeCoders.Net.Soap;
using CreativeCoders.Net.WebRequests;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm.Tr064;

[PublicAPI]
public class HostsApi : Tr064ApiBase
{
    public HostsApi(string fritzBoxUrl, string userName, string password) : this(new SoapHttpClient(WebRequestFactory.Default), fritzBoxUrl, userName, password) { }

    public HostsApi(ISoapHttpClient soapHttpClient, string fritzBoxUrl, string userName, string password) : base(soapHttpClient, fritzBoxUrl, "/upnp/control/hosts", userName, password) { }


    public GetHostNumberOfEntriesResponse GetHostNumberOfEntries()
    {
        return SoapHttpClient.Invoke<GetHostNumberOfEntriesRequest, GetHostNumberOfEntriesResponse>(new GetHostNumberOfEntriesRequest());
    }

    public GetSpecificHostEntryResponse GetSpecificHostEntry(string macAddress)
    {
        return SoapHttpClient.Invoke<GetSpecificHostEntryRequest, GetSpecificHostEntryResponse>(
            new GetSpecificHostEntryRequest {MacAddress = macAddress});
    }

    public GetGenericHostEntryResponse GetGenericHostEntry(int index)
    {
        return SoapHttpClient.Invoke<GetGenericHostEntryRequest, GetGenericHostEntryResponse>(
            new GetGenericHostEntryRequest {Index = index});
    }
}