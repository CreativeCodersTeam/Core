using System;
using System.Collections.Generic;

namespace CreativeCoders.SysConsole.CliArguments.Commands
{
    public class CliCommandGroup : ICliCommandGroup
    {
        public string Name { get; set; } = string.Empty;

        public IEnumerable<ICliCommand> Commands { get; init; } = Array.Empty<ICliCommand>();
    }
}
