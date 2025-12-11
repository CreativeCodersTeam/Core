using CreativeCoders.Core;

namespace CreativeCoders.Cli.Hosting.Commands.Store;

public class CliTreeNode(string name, CliCommandGroupNode? parent)
{
    public IEnumerable<string> GetNamePath()
    {
        if (Parent != null)
        {
            foreach (var namePart in Parent.GetNamePath())
            {
                yield return namePart;
            }
        }

        yield return Name;
    }

    public List<CliTreeNode> ChildNodes { get; } = new List<CliTreeNode>();

    public CliCommandGroupNode? Parent { get; } = parent;

    public string Name { get; } = Ensure.IsNotNullOrWhitespace(name);
}
