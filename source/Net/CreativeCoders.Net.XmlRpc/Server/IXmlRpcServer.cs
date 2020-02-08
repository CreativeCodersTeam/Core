using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Net.XmlRpc.Server
{
    [PublicAPI]
    public interface IXmlRpcServer
    {
        Task StartAsync();

        Task StopAsync();

        IList<string> Urls { get; }

        IXmlRpcServerMethods Methods { get; }

        bool SupportsListMethods { get; set; }
    }
}