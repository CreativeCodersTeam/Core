using System.Collections.Generic;

namespace CreativeCoders.SysConsole.CliArguments.Commands
{
    public interface ICliCommandGroup
    {
        public string Name { get; set; }

        IEnumerable<ICliCommand> Commands { get; }
    }
}
