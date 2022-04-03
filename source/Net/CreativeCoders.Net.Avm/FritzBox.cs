using System.Net.Http;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm;

[PublicAPI]
public class FritzBox
{
    public FritzBox(IHttpClientFactory httpClientFactory, string url) : this(httpClientFactory, url, string.Empty, string.Empty) { }

    public FritzBox(IHttpClientFactory httpClientFactory, string url, string userName, string password)
    {
        Ensure.IsNotNullOrWhitespace(url, nameof(url));

        Hosts = new Hosts(httpClientFactory, url, userName, password);
        WanPppConnection = new WanPppConnection(httpClientFactory, url, userName, password);
        Wlan = new Wlan(httpClientFactory, url, userName, password);
    }

    public Hosts Hosts { get; }

    public WanPppConnection WanPppConnection { get; }

    public Wlan Wlan { get; }
}
