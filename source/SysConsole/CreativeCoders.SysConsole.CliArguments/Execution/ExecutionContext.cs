using System.Collections.Generic;
using CreativeCoders.Core;
using CreativeCoders.SysConsole.CliArguments.Commands;

namespace CreativeCoders.SysConsole.CliArguments.Execution
{
    public class ExecutionContext
    {
        public ExecutionContext(IEnumerable<ICliCommandGroup> commandGroups,
            IEnumerable<ICliCommand> commands, ICliCommand? defaultCommand)
        {
            CommandGroups = Ensure.NotNull(commandGroups, nameof(commandGroups));
            Commands = Ensure.NotNull(commands, nameof(commands));
            DefaultCommand = defaultCommand;
        }

        public IEnumerable<ICliCommandGroup> CommandGroups { get; }

        public IEnumerable<ICliCommand> Commands { get; }

        public ICliCommand? DefaultCommand { get; }
    }
}
