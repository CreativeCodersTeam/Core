using CreativeCoders.Cli.Core;

namespace CreativeCoders.Cli.Hosting.Commands.Store;

public interface ICliCommandStore
{
    void AddCommands(IEnumerable<CliCommandInfo> commands,
        IEnumerable<CliCommandGroupAttribute>? groupAttributes =
            null);

    FindCommandNodeResult<CliCommandGroupNode>? FindCommandGroupNode(string[] args);

    FindCommandNodeResult<CliCommandNode>? FindCommandNode(string[] args);

    IEnumerable<CliTreeNode> TreeRootNodes { get; }

    IEnumerable<CliCommandInfo> Commands { get; }

    IEnumerable<CliCommandGroupAttribute> GroupAttributes { get; }
}
