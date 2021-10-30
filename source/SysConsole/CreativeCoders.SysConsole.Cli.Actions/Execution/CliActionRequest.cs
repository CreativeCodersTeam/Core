using System.Collections.Generic;

namespace CreativeCoders.SysConsole.Cli.Actions.Execution
{
    public class CliActionRequest
    {
        public CliActionRequest(string[] args)
        {
            Arguments = args;
        }

        public IList<string> Arguments { get; set; }
    }
}
