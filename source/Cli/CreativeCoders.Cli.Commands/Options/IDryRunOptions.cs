using JetBrains.Annotations;

namespace CreativeCoders.Cli.Commands.Options;

/// <summary>
/// Marker interface implemented by option types that support a dry-run mode.
/// When <see cref="DryRun"/> is <see langword="true"/>, the base class skips
/// <c>OnExecuteAsync</c> and invokes the dry-run hook instead.
/// </summary>
[PublicAPI]
public interface IDryRunOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether the command should be executed in dry-run mode
    /// (no side effects).
    /// </summary>
    bool DryRun { get; set; }
}
