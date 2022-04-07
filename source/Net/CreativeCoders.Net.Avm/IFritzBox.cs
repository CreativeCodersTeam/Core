using CreativeCoders.Net.Avm.Hosts;
using CreativeCoders.Net.Avm.Wan;
using CreativeCoders.Net.Avm.Wlan;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm;

[PublicAPI]
public interface IFritzBox
{
    public IHosts Hosts { get; }

    public IWanPppConnection WanPppConnection { get; }

    public IWlan Wlan { get; }
}
