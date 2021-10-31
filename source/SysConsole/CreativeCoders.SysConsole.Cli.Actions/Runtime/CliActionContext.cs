using System.Collections.Generic;
using CreativeCoders.SysConsole.Cli.Actions.Routing;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime
{
    [PublicAPI]
    public class CliActionContext
    {
        public CliActionContext(CliActionRequest request)
        {
            Request = request;

            Arguments = request.Arguments;
        }

        public CliActionRequest Request { get; }

        public CliActionRoute? ActionRoute { get; set; }

        public IList<string> Arguments { get; set; }

        public int ReturnCode { get; set; }
    }
}
