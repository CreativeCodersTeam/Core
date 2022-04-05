using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CreativeCoders.Net.Avm;

public interface IFritzBoxFactory
{
    IFritzBox Create(string name);
}

public class FritzBoxFactory : IFritzBoxFactory
{
    private readonly IFritzBoxConnections _fritzBoxConnections;

    public FritzBoxFactory(IFritzBoxConnections fritzBoxConnections)
    {
        _fritzBoxConnections = fritzBoxConnections;
    }

    public IFritzBox Create(string name)
    {
        throw new NotImplementedException();
    }
}

public interface IFritzBox
{
    public Hosts Hosts { get; }

    public WanPppConnection WanPppConnection { get; }

    public Wlan Wlan { get; }
}

//public interface IFritzBoxHosts
//{
//    Task<HostEntry> GetHostEntryAsync(string macAddress);

//    Task<int> GetHostCountAsync();

//    Task<HostEntry> GetHostEntryAsync(int index);

//    Task<IEnumerable<HostEntry>> GetAllHostEntriesAsync();
//}
