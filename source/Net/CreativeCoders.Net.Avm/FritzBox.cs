using System.Net.Http;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm;

[PublicAPI]
public class FritzBox : IFritzBox
{
    public FritzBox(HttpClient httpClient)
    {
        Hosts = new Hosts(httpClient);
        WanPppConnection = new WanPppConnection(httpClient);
        Wlan = new Wlan(httpClient);
    }

    public Hosts Hosts { get; }

    public WanPppConnection WanPppConnection { get; }

    public Wlan Wlan { get; }
}
