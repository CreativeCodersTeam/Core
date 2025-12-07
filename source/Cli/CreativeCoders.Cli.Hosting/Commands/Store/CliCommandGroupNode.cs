using CreativeCoders.Core;

namespace CreativeCoders.Cli.Hosting.Commands.Store;

public class CliCommandGroupNode(string groupName, CliCommandGroupNode? parent)
    : CliTreeNode(groupName, parent)
{
    public IEnumerable<CliCommandNode> GetCommands() => ChildNodes.OfType<CliCommandNode>();

    public IEnumerable<CliCommandGroupNode> GetSubCommandGroups() => ChildNodes.OfType<CliCommandGroupNode>();
}
