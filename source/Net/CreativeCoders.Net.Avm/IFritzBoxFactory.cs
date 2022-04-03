using System.Collections.Generic;
using System.Threading.Tasks;

namespace CreativeCoders.Net.Avm;

public interface IFritzBoxFactory
{
    //IFritzBox Create(string )
}

public interface IFritzBox
{

}

public interface IFritzBoxHosts
{
    Task<HostEntry> GetHostEntryAsync(string macAddress);

    Task<int> GetHostCountAsync();

    Task<HostEntry> GetHostEntryAsync(int index);

    Task<IEnumerable<HostEntry>> GetAllHostEntriesAsync();
}
