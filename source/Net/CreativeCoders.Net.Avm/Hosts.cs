﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CreativeCoders.Core;
using CreativeCoders.Net.Avm.Tr064;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm;

[PublicAPI]
public class Hosts
{
    private readonly HostsApi _hostsApi;

    public Hosts(HttpClient httpClient, string url)
    {
        Ensure.IsNotNullOrWhitespace(url, nameof(url));

        _hostsApi = new HostsApi(httpClient, url);
    }

    public async Task<HostEntry> GetHostEntryAsync(string macAddress)
    {
        Ensure.IsNotNullOrWhitespace(macAddress, nameof(macAddress));

        var response = await _hostsApi.GetSpecificHostEntryAsync(macAddress).ConfigureAwait(false);

        var entry = new HostEntry
        {
            MacAddress = macAddress,
            IpAddress = response.IpAddress,
            AddressSource = response.AddressSource,
            LeaseTimeRemaining = response.LeaseTimeRemaining,
            InterfaceType = response.InterfaceType,
            IsActive = response.Active == 1,
            HostName = response.HostName
        };

        return entry;
    }

    public async Task<int> GetHostCountAsync()
    {
        var response = await _hostsApi.GetHostNumberOfEntriesAsync().ConfigureAwait(false);
        return response?.HostNumberOfEntries ?? 0;
    }

    public async Task<HostEntry> GetHostEntryAsync(int index)
    {
        var response = await _hostsApi.GetGenericHostEntryAsync(index).ConfigureAwait(false);

        var entry = new HostEntry
        {
            MacAddress = response.MacAddress,
            IpAddress = response.IpAddress,
            AddressSource = response.AddressSource,
            LeaseTimeRemaining = response.LeaseTimeRemaining,
            InterfaceType = response.InterfaceType,
            IsActive = response.Active == 1,
            HostName = response.HostName
        };

        return entry;
    }

    public async Task<IEnumerable<HostEntry>> GetAllHostEntriesAsync()
    {
        return (await InternalGetAllHostEntriesAsync().ConfigureAwait(false))
            .ToArray();
    }

    public async Task<IEnumerable<HostEntry>> GetAllHostEntriesAsync(bool hostIsActive)
    {
        return (await InternalGetAllHostEntriesAsync().ConfigureAwait(false))
            .Where(host => host.IsActive == hostIsActive)
            .ToArray();
    }

    private async Task<IEnumerable<HostEntry>> InternalGetAllHostEntriesAsync()
    {
        var hostCount = await GetHostCountAsync();

        var hosts = new List<HostEntry>();

        for (var i = 0; i < hostCount; i++)
        {
            hosts.Add(await GetHostEntryAsync(i));
            //yield return await GetHostEntryAsync(i);
        }

        return hosts;
    }

    //private async Task<IEnumerable<HostEntry>> InternalGetAllHostEntriesAsync()
    //{
    //    var hostCount = GetHostCount();

    //    for (var i = 0; i < hostCount; i++)
    //    {
    //        yield return GetHostEntry(i);
    //    }
    //}
}
