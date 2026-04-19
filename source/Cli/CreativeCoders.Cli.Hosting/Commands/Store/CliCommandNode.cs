using CreativeCoders.Core;

namespace CreativeCoders.Cli.Hosting.Commands.Store;

/// <summary>
/// Represents a leaf node in the CLI command tree that holds a specific command.
/// </summary>
/// <param name="commandInfo">The command information associated with this node.</param>
/// <param name="command">The command name segment.</param>
/// <param name="parent">The parent group node, or <see langword="null"/> if this is a root command.</param>
public class CliCommandNode(CliCommandInfo commandInfo, string command, CliCommandGroupNode? parent)
    : CliTreeNode(command, parent)
{
    /// <summary>
    /// Gets the command information associated with this node.
    /// </summary>
    /// <value>The <see cref="CliCommandInfo"/> for this command.</value>
    public CliCommandInfo CommandInfo { get; } = Ensure.NotNull(commandInfo);
}
