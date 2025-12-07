using CreativeCoders.Core;

namespace CreativeCoders.Cli.Hosting.Commands.Store;

public class CliCommandNode(CliCommandInfo commandInfo, string command, CliCommandGroupNode? parent)
    : CliTreeNode(command, parent)
{
    public CliCommandInfo CommandInfo { get; } = Ensure.NotNull(commandInfo);
}
