using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm
{
    [PublicAPI]
    public class FritzBox
    {
        public FritzBox(string url) : this(url, string.Empty, string.Empty) { }

        public FritzBox(string url, string userName, string password)
        {
            Ensure.IsNotNullOrWhitespace(url, nameof(url));

            Hosts = new Hosts(url, userName, password);
            WanPppConnection = new WanPppConnection(url, userName, password);
        }

        public Hosts Hosts { get; }

        public WanPppConnection WanPppConnection { get; }
    }
}
