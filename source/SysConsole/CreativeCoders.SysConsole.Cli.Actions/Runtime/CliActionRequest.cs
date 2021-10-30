using System.Collections.Generic;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime
{
    public class CliActionRequest
    {
        public CliActionRequest(string[] args)
        {
            Arguments = args;
        }

        public string[] Arguments { get; set; }
    }
}
