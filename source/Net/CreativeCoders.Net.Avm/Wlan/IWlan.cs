using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Avm.Wlan;

[PublicAPI]
public interface IWlan
{
    Task<WlanDeviceInfo> GetWlanDeviceInfoAsync(string macAddress);
}
