using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Net
{
    [PublicAPI]
    public interface INetUtils {
        int FindFreePort(IEnumerable<int> portRange);

        int FindFreePort(int startPort);

        int NoFreePortFound { get; } 
    }
}