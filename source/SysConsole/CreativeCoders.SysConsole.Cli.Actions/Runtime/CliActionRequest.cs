using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.Runtime
{
    [PublicAPI]
    public class CliActionRequest
    {
        public CliActionRequest(string[] args)
        {
            Arguments = args;
        }

        public string[] Arguments { get; set; }
    }
}
