using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Net;

[PublicAPI]
public class NetworkInfo : INetworkInfo
{
    public int FindFreePort(IEnumerable<int> portRange)
    {
        var ports = portRange?.ToArray();
        Ensure.IsNotNullOrEmpty(ports, nameof(ports));

        var ipProperties = IPGlobalProperties.GetIPGlobalProperties();
        var ipEndpoints = ipProperties.GetActiveTcpListeners().Concat(ipProperties.GetActiveUdpListeners())
            .Distinct().ToArray();

        foreach (var port in ports.Where(port => ipEndpoints.All(endpoint => endpoint.Port != port)))
        {
            return port;
        }

        return NoFreePortFound;
    }

    public int FindFreePort(int startPort)
    {
        const int maxPort = IPEndPoint.MaxPort;
            
        return FindFreePort(Enumerable.Range(startPort, maxPort - startPort + 1));
    }

    public int NoFreePortFound => -1;

    public string GetHostName()
    {
        return IPGlobalProperties.GetIPGlobalProperties().HostName;
    }

    public string GetDomainName()
    {
        return IPGlobalProperties.GetIPGlobalProperties().DomainName;
    }
}