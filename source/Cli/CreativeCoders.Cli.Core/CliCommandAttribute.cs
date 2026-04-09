using CreativeCoders.Core;

namespace CreativeCoders.Cli.Core;

/// <summary>
/// Marks a class as a CLI command and specifies the command path used to invoke it.
/// </summary>
/// <param name="commands">The command path segments used to invoke this command.</param>
[AttributeUsage(AttributeTargets.Class)]
public class CliCommandAttribute(string[] commands) : Attribute
{
    /// <summary>
    /// Gets or sets the display name of the command.
    /// </summary>
    /// <value>The display name of the command. The default is <see cref="string.Empty"/>.</value>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets the command path segments used to invoke this command.
    /// </summary>
    /// <value>An array of strings representing the command path.</value>
    public string[] Commands { get; } = Ensure.NotNull(commands);

    /// <summary>
    /// Gets or sets the description of the command displayed in help output.
    /// </summary>
    /// <value>The description text. The default is <see cref="string.Empty"/>.</value>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the alternative command path segments that can also invoke this command.
    /// </summary>
    /// <value>An array of alternative command path segments. The default is an empty array.</value>
    public string[] AlternativeCommands { get; init; } = [];
}
