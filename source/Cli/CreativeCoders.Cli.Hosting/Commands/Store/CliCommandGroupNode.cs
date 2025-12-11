using CreativeCoders.Cli.Core;

namespace CreativeCoders.Cli.Hosting.Commands.Store;

public class CliCommandGroupNode(string groupName, CliCommandGroupNode? parent)
    : CliTreeNode(groupName, parent)
{
    public CliCommandGroupAttribute? GroupAttribute { get; set; }

    public IEnumerable<CliCommandNode> GetCommands() => ChildNodes.OfType<CliCommandNode>();

    public IEnumerable<CliCommandGroupNode> GetSubCommandGroups() => ChildNodes.OfType<CliCommandGroupNode>();
}
