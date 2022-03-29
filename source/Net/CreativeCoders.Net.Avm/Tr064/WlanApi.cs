using System.Net;
using CreativeCoders.Net.Avm.Tr064.Exceptions;
using CreativeCoders.Net.Avm.Tr064.Wlan.Requests;
using CreativeCoders.Net.Avm.Tr064.Wlan.Responses;
using CreativeCoders.Net.Soap;
using CreativeCoders.Net.WebRequests;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm.Tr064;

[PublicAPI]
public class WlanApi : Tr064ApiBase
{
    public WlanApi(string fritzBoxUrl, string userName, string password) : this(
        new SoapHttpClient(WebRequestFactory.Default), fritzBoxUrl, userName, password) { }

    public WlanApi(ISoapHttpClient soapHttpClient, string fritzBoxUrl, string userName, string password)
        // ReSharper disable once StringLiteralTypo
        : base(soapHttpClient, fritzBoxUrl, "/upnp/control/wlanconfig1", userName, password) { }

    public GetSpecificAssociatedDeviceInfoResponse GetSpecificAssociatedDeviceInfo(string macAddress)
    {
        try
        {
            return SoapHttpClient
                .Invoke<GetSpecificAssociatedDeviceInfoRequest, GetSpecificAssociatedDeviceInfoResponse>(
                    new GetSpecificAssociatedDeviceInfoRequest {MacAddress = macAddress});
        }
        catch (WebException e)
        {
            throw new EntryNotFoundException(macAddress, $"Wlan device info for '{macAddress}' not found.",
                e);
        }
    }
}
