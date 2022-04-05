using System.Net.Http;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm;

[PublicAPI]
public class FritzBox : IFritzBox
{
    public FritzBox(HttpClient httpClient, string url)
    {
        Ensure.IsNotNullOrWhitespace(url, nameof(url));

        Hosts = new Hosts(httpClient, url);
        WanPppConnection = new WanPppConnection(httpClient, url);
        Wlan = new Wlan(httpClient, url);
    }

    public Hosts Hosts { get; }

    public WanPppConnection WanPppConnection { get; }

    public Wlan Wlan { get; }
}
