using CreativeCoders.Core;

namespace CreativeCoders.Cli.Hosting.Commands.Store;

public class CliTreeNode(string name)
{
    public List<CliTreeNode> ChildNodes { get; } = new List<CliTreeNode>();

    public CliCommandGroupNode? Parent { get; set; }

    public string Name { get; } = Ensure.IsNotNullOrWhitespace(name);
}
