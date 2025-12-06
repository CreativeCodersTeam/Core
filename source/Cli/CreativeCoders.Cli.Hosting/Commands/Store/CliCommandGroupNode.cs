using CreativeCoders.Core;

namespace CreativeCoders.Cli.Hosting.Commands.Store;

public class CliCommandGroupNode(string groupName) : CliTreeNode(groupName)
{
    public IEnumerable<CliCommandNode> GetCommands() => ChildNodes.OfType<CliCommandNode>();

    public IEnumerable<CliCommandGroupNode> GetSubCommandGroups() => ChildNodes.OfType<CliCommandGroupNode>();
}
