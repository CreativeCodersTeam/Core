using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Server;

[PublicAPI]
public interface IXmlRpcServer : IDisposable
{
    Task StartAsync();

    Task StopAsync();

    IList<string> Urls { get; }

    IXmlRpcServerMethods Methods { get; }

    bool SupportsListMethods { get; set; }

    Encoding Encoding { get; set; }
}
