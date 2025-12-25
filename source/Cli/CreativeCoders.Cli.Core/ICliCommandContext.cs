using JetBrains.Annotations;

namespace CreativeCoders.Cli.Core;

/// <summary>
/// Represents the context for a CLI command, providing access to arguments passed to the command.
/// </summary>
[PublicAPI]
public interface ICliCommandContext
{
    /// <summary>
    /// Gets or sets all arguments passed to the CLI app, including commands, options and positional arguments.
    /// This property provides full visibility of the input arguments passed to the CLI app for further processing.
    /// </summary>
    public string[] AllArgs { get; set; }

    /// <summary>
    /// Gets or sets the arguments that represent options passed to the CLI command.
    /// This property contains only the arguments classified as options and excludes CLI inputs such as commands.
    /// </summary>
    public string[] OptionsArgs { get; set; }
}
