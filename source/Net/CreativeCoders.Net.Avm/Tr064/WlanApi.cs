﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.Net.Avm.Tr064.Exceptions;
using CreativeCoders.Net.Avm.Tr064.Wlan.Requests;
using CreativeCoders.Net.Avm.Tr064.Wlan.Responses;
using CreativeCoders.Net.Soap;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm.Tr064;

[PublicAPI]
public class WlanApi : Tr064ApiBase
{
    public WlanApi(IHttpClientFactory httpClientFactory, string fritzBoxUrl, string userName, string password) : this(
        new SoapHttpClient(httpClientFactory), fritzBoxUrl, userName, password) { }

    public WlanApi(ISoapHttpClient soapHttpClient, string fritzBoxUrl, string userName, string password)
        // ReSharper disable once StringLiteralTypo
        : base(soapHttpClient, fritzBoxUrl, "/upnp/control/wlanconfig1", userName, password) { }

    public async Task<GetSpecificAssociatedDeviceInfoResponse> GetSpecificAssociatedDeviceInfoAsync(string macAddress)
    {
        try
        {
            return await SoapHttpClient
                .InvokeAsync<GetSpecificAssociatedDeviceInfoRequest, GetSpecificAssociatedDeviceInfoResponse>(
                    new GetSpecificAssociatedDeviceInfoRequest {MacAddress = macAddress})
                .ConfigureAwait(false);
        }
        catch (WebException e)
        {
            throw new EntryNotFoundException(macAddress, $"Wlan device info for '{macAddress}' not found.",
                e);
        }
    }
}
