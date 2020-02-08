using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core;
using CreativeCoders.Net.Avm.Tr064;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm
{
    [PublicAPI]
    public class Hosts
    {
        private readonly HostsApi _hostsApi;

        public Hosts(string url, string userName, string password)
        {
            Ensure.IsNotNullOrWhitespace(url, nameof(url));

            _hostsApi = new HostsApi(url, userName, password);            
        }

        public HostEntry GetHostEntry(string macAddress)
        {
            Ensure.IsNotNullOrWhitespace(macAddress, nameof(macAddress));

            var response = _hostsApi.GetSpecificHostEntry(macAddress);

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

        public int GetHostCount()
        {
            var response = _hostsApi.GetHostNumberOfEntries();
            return response?.HostNumberOfEntries ?? 0;
        }

        public HostEntry GetHostEntry(int index)
        {
            var response = _hostsApi.GetGenericHostEntry(index);

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

        public IEnumerable<HostEntry> GetAllHostEntries()
        {
            return InternalGetAllHostEntries().ToArray();
        }

        public IEnumerable<HostEntry> GetAllHostEntries(bool hostIsActive)
        {
            return InternalGetAllHostEntries().Where(host => host.IsActive == hostIsActive).ToArray();
        }

        private IEnumerable<HostEntry> InternalGetAllHostEntries()
        {
            var hostCount = GetHostCount();

            for (var i = 0; i < hostCount; i++)
            {
                yield return GetHostEntry(i);
            }
        }
    }
}