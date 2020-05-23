using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Net
{
    [PublicAPI]
    public interface INetworkInfo
    {
        int FindFreePort(IEnumerable<int> portRange);

        int FindFreePort(int startPort);

        int NoFreePortFound { get; }

        string GetHostName();

        string GetDomainName();
    }
}