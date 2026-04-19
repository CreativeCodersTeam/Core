using JetBrains.Annotations;

namespace CreativeCoders.Cli.Core;

/// <summary>
/// Represents the result of a CLI application execution.
/// </summary>
/// <param name="exitCode">The exit code of the CLI execution.</param>
[PublicAPI]
public class CliResult(int exitCode)
{
    /// <summary>
    /// Gets or sets the exit code of the CLI execution.
    /// </summary>
    /// <value>The exit code.</value>
    public int ExitCode { get; set; } = exitCode;
}
