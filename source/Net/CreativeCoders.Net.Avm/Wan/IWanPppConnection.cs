using System.Net;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm.Wan;

[PublicAPI]
public interface IWanPppConnection
{
    Task<string> GetExternalIpAddressAsync();

    Task<IPAddress> GetExternalIpAddressExAsync();
}
