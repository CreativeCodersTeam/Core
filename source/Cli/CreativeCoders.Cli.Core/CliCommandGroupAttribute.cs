namespace CreativeCoders.Cli.Core;

/// <summary>
/// Defines a command group that organizes related CLI commands under a common path.
/// </summary>
/// <param name="commands">The command path segments that identify this group.</param>
/// <param name="description">The description of the command group displayed in help output.</param>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public class CliCommandGroupAttribute(string[] commands, string description) : Attribute
{
    /// <summary>
    /// Gets the command path segments that identify this group.
    /// </summary>
    /// <value>An array of strings representing the group command path.</value>
    public string[] Commands { get; } = commands;

    /// <summary>
    /// Gets the description of the command group displayed in help output.
    /// </summary>
    /// <value>The description text.</value>
    public string Description { get; } = description;
}
