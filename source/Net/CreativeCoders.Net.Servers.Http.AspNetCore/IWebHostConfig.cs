using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Net.Servers.Http.AspNetCore
{
    [PublicAPI]
    public interface IWebHostConfig
    {
        IReadOnlyCollection<string> Urls { get; }
        
        bool DisableLogging { get; set; }

        bool AllowSynchronousIO { get; set; }
    }
}