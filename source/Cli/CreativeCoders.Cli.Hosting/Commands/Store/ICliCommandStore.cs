using CreativeCoders.Cli.Core;

namespace CreativeCoders.Cli.Hosting.Commands.Store;

/// <summary>
/// Defines a store for CLI commands organized in a tree structure.
/// </summary>
public interface ICliCommandStore
{
    /// <summary>
    /// Adds commands and optional group attributes to the store.
    /// </summary>
    /// <param name="commands">The collection of command information objects to add.</param>
    /// <param name="groupAttributes">An optional collection of group attributes for organizing commands.</param>
    void AddCommands(IEnumerable<CliCommandInfo> commands,
        IEnumerable<CliCommandGroupAttribute>? groupAttributes =
            null);

    /// <summary>
    /// Searches for a command group node matching the provided arguments.
    /// </summary>
    /// <param name="args">The command line arguments to match against the command tree.</param>
    /// <returns>A <see cref="FindCommandNodeResult{TNode}"/> containing the matched group node and remaining args, or <see langword="null"/> if no match was found.</returns>
    FindCommandNodeResult<CliCommandGroupNode>? FindCommandGroupNode(string[] args);

    /// <summary>
    /// Searches for a command node matching the provided arguments.
    /// </summary>
    /// <param name="args">The command line arguments to match against the command tree.</param>
    /// <returns>A <see cref="FindCommandNodeResult{TNode}"/> containing the matched command node and remaining args, or <see langword="null"/> if no match was found.</returns>
    FindCommandNodeResult<CliCommandNode>? FindCommandNode(string[] args);

    /// <summary>
    /// Gets the root nodes of the command tree.
    /// </summary>
    /// <value>An enumerable of root <see cref="CliTreeNode"/> instances.</value>
    IEnumerable<CliTreeNode> TreeRootNodes { get; }

    /// <summary>
    /// Gets all registered command information objects.
    /// </summary>
    /// <value>An enumerable of <see cref="CliCommandInfo"/> instances.</value>
    IEnumerable<CliCommandInfo> Commands { get; }

    /// <summary>
    /// Gets all registered command group attributes.
    /// </summary>
    /// <value>An enumerable of <see cref="CliCommandGroupAttribute"/> instances.</value>
    IEnumerable<CliCommandGroupAttribute> GroupAttributes { get; }
}
