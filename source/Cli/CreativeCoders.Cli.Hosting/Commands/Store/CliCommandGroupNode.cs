using CreativeCoders.Cli.Core;

namespace CreativeCoders.Cli.Hosting.Commands.Store;

/// <summary>
/// Represents a command group node in the CLI command tree that contains child commands or subgroups.
/// </summary>
/// <param name="groupName">The name of the command group.</param>
/// <param name="parent">The parent group node, or <see langword="null"/> if this is a root group.</param>
public class CliCommandGroupNode(string groupName, CliCommandGroupNode? parent)
    : CliTreeNode(groupName, parent)
{
    /// <summary>
    /// Gets or sets the attribute that provides metadata for this command group.
    /// </summary>
    /// <value>The <see cref="CliCommandGroupAttribute"/> associated with this group, or <see langword="null"/> if no attribute is defined.</value>
    public CliCommandGroupAttribute? GroupAttribute { get; set; }
}
