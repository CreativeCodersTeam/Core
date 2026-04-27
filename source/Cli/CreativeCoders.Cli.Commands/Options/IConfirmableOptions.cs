using JetBrains.Annotations;

namespace CreativeCoders.Cli.Commands.Options;

/// <summary>
/// Marker interface implemented by option types that support an interactive confirmation step.
/// The base class prompts for confirmation unless <see cref="Yes"/> is <see langword="true"/>.
/// </summary>
[PublicAPI]
public interface IConfirmableOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether the confirmation prompt should be skipped
    /// (the user has pre-approved the action, typically via <c>--yes</c>).
    /// </summary>
    bool Yes { get; set; }
}
