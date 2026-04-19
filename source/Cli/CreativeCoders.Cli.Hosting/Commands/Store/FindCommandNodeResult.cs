using CreativeCoders.Core;

namespace CreativeCoders.Cli.Hosting.Commands.Store;

/// <summary>
/// Represents the result of searching for a node in the CLI command tree.
/// </summary>
/// <typeparam name="TNode">The type of the tree node found.</typeparam>
/// <param name="node">The found tree node, or <see langword="null"/> if no match was found.</param>
/// <param name="remainingArgs">The arguments that were not consumed during the search.</param>
public class FindCommandNodeResult<TNode>(TNode? node, string[] remainingArgs)
    where TNode : CliTreeNode
{
    /// <summary>
    /// Gets the found tree node.
    /// </summary>
    /// <value>The matched <typeparamref name="TNode"/>, or <see langword="null"/> if no match was found.</value>
    public TNode? Node { get; } = node;

    /// <summary>
    /// Gets the arguments that were not consumed during the command search.
    /// </summary>
    /// <value>An array of remaining argument strings.</value>
    public string[] RemainingArgs { get; } = Ensure.NotNull(remainingArgs);
}
