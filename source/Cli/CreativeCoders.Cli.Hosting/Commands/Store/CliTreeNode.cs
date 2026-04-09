using CreativeCoders.Core;

namespace CreativeCoders.Cli.Hosting.Commands.Store;

/// <summary>
/// Represents a node in the CLI command tree structure.
/// </summary>
/// <param name="name">The name of this tree node.</param>
/// <param name="parent">The parent group node, or <see langword="null"/> if this is a root node.</param>
public class CliTreeNode(string name, CliCommandGroupNode? parent)
{
    /// <summary>
    /// Returns the full name path from the root to this node.
    /// </summary>
    /// <returns>An enumerable of name segments from root to this node.</returns>
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

    /// <summary>
    /// Gets the child nodes of this tree node.
    /// </summary>
    /// <value>A list of child <see cref="CliTreeNode"/> instances.</value>
    public List<CliTreeNode> ChildNodes { get; } = [];

    /// <summary>
    /// Gets the parent group node.
    /// </summary>
    /// <value>The parent <see cref="CliCommandGroupNode"/>, or <see langword="null"/> if this is a root node.</value>
    public CliCommandGroupNode? Parent { get; } = parent;

    /// <summary>
    /// Gets the name of this tree node.
    /// </summary>
    /// <value>The node name.</value>
    public string Name { get; } = Ensure.IsNotNullOrWhitespace(name);
}
