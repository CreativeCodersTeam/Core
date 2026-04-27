using JetBrains.Annotations;

namespace CreativeCoders.Cli.Commands.Options;

/// <summary>
/// Marker interface implemented by option types that opt in to interactive prompting for
/// missing required values when running on a TTY.
/// </summary>
[PublicAPI]
public interface IInteractiveOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether interactive prompting should be suppressed,
    /// even on a TTY. Useful for non-interactive automation scenarios.
    /// </summary>
    bool NoInteractive { get; set; }
}
