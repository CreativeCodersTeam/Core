using CreativeCoders.Cli.Core;

namespace CreativeCoders.Cli.Hosting.Commands;

/// <summary>
/// Holds metadata about a registered CLI command, including its attribute, type, and options type.
/// </summary>
public class CliCommandInfo
{
    /// <summary>
    /// Gets the attribute that defines the command's name, path, and description.
    /// </summary>
    /// <value>The <see cref="CliCommandAttribute"/> applied to the command class.</value>
    public required CliCommandAttribute CommandAttribute { get; init; }

    /// <summary>
    /// Gets the type of the command class.
    /// </summary>
    /// <value>The <see cref="Type"/> of the CLI command implementation.</value>
    public required Type CommandType { get; init; }

    /// <summary>
    /// Gets the type of the options class used by the command, if any.
    /// </summary>
    /// <value>The <see cref="Type"/> of the options class, or <see langword="null"/> if the command has no options.</value>
    public Type? OptionsType { get; init; }
}
