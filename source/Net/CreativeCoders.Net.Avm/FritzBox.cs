using System.Net.Http;
using CreativeCoders.Net.Avm.Hosts;
using CreativeCoders.Net.Avm.Wan;
using CreativeCoders.Net.Avm.Wlan;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm;

[PublicAPI]
public class FritzBox : IFritzBox
{
    public FritzBox(HttpClient httpClient)
    {
        Hosts = new HostsImpl(httpClient);
        WanPppConnection = new WanPppConnectionImpl(httpClient);
        Wlan = new WlanImpl(httpClient);
    }

    public IHosts Hosts { get; }

    public IWanPppConnection WanPppConnection { get; }

    public IWlan Wlan { get; }
}
