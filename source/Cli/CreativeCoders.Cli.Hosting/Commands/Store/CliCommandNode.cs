using CreativeCoders.Core;

namespace CreativeCoders.Cli.Hosting.Commands.Store;

public class CliCommandNode(CliCommandInfo commandInfo, string command) : CliTreeNode(command)
{
    public CliCommandInfo CommandInfo { get; } = Ensure.NotNull(commandInfo);
}
