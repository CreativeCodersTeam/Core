namespace CreativeCoders.Cli.Hosting.Commands.Store;

public interface ICliCommandStore
{
    void AddCommands(IEnumerable<CliCommandInfo> commands);

    FindCommandNodeResult<CliCommandGroupNode>? FindCommandGroupNode(string[] args);

    FindCommandNodeResult<CliCommandNode>? FindCommandNode(string[] args);

    IEnumerable<CliTreeNode> TreeRootNodes { get; }

    IEnumerable<CliCommandInfo> Commands { get; }
}
