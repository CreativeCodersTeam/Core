using System.Collections.Generic;
using CreativeCoders.SysConsole.CliArguments.Commands;

namespace CreativeCoders.SysConsole.CliArguments.Execution
{
    public class ExecutionContext
    {
        public ExecutionContext(IEnumerable<ICliCommandGroup> commandGroups,
            IEnumerable<ICliCommand> commands, ICliCommand? defaultCommand)
        {
            CommandGroups = commandGroups;
            Commands = commands;
            DefaultCommand = defaultCommand;
        }

        public IEnumerable<ICliCommandGroup> CommandGroups { get; }

        public IEnumerable<ICliCommand> Commands { get; }

        public ICliCommand? DefaultCommand { get; }
    }
}
