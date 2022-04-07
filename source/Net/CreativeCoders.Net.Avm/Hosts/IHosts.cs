using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm.Hosts;

[PublicAPI]
public interface IHosts
{
    Task<HostEntry> GetHostEntryAsync(string macAddress);

    Task<HostEntry> GetHostEntryAsync(int index);

    Task<int> GetHostCountAsync();

    Task<IEnumerable<HostEntry>> GetAllHostEntriesAsync();

    Task<IEnumerable<HostEntry>> GetAllHostEntriesAsync(bool hostIsActive);
}
