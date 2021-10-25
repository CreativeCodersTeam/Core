using System;
using System.Collections.Generic;

namespace CreativeCoders.SysConsole.CliArguments.Commands
{
    public interface ICliCommandGroup
    {
        public string Name { get; set; }

        IEnumerable<ICliCommand> Commands { get; }
    }

    public class CliCommandGroup : ICliCommandGroup
    {
        public string Name { get; set; } = string.Empty;

        public IEnumerable<ICliCommand> Commands { get; set; } = Array.Empty<ICliCommand>();
    }
}
