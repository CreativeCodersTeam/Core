using JetBrains.Annotations;

namespace CreativeCoders.Cli.Commands.Options;

/// <summary>
/// Marker interface implemented by option types that carry a <see cref="Verbosity"/> setting.
/// </summary>
[PublicAPI]
public interface IVerbosityOptions
{
    /// <summary>
    /// Gets or sets the desired verbosity level for the command.
    /// </summary>
    Verbosity Verbosity { get; set; }
}
