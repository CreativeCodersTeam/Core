namespace CreativeCoders.Cli.Hosting.Commands.Store;

public interface ICliCommandStore
{
    void AddCommands(IEnumerable<CliCommandInfo> commands);

    CliCommandInfo? FindCommandForArgs(string[] args);

    CliCommandGroupNode? FindGroupNodeForArgs(string[] args);

    IEnumerable<CliTreeNode> TreeRootNodes { get; }

    IEnumerable<CliCommandInfo> Commands { get; }
}
