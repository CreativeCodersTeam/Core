using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime
{
    [PublicAPI]
    public class CliActionRequest
    {
        public CliActionRequest(string[] args)
        {
            Arguments = args.ToList();
        }

        public IList<string> Arguments { get; set; }
    }
}
