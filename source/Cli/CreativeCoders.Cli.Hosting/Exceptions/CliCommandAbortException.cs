using JetBrains.Annotations;

namespace CreativeCoders.Cli.Hosting.Exceptions;

/// <summary>
/// Represents an exception thrown when a CLI command is intentionally aborted.
/// </summary>
/// <param name="message">The abort message.</param>
/// <param name="exitCode">The exit code to return.</param>
/// <param name="exception">The optional inner exception.</param>
[PublicAPI]
public class CliCommandAbortException(string message, int exitCode, Exception? exception = null)
    : CliExitException(message, exitCode, exception)
{
    /// <summary>
    /// Gets or sets a value indicating whether this abort is treated as an error.
    /// </summary>
    /// <value><see langword="true"/> if the abort is an error; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
    public bool IsError { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the abort message should be printed to the console.
    /// </summary>
    /// <value><see langword="true"/> if the message should be printed; otherwise, <see langword="false"/>. The default is <see langword="true"/>.</value>
    public bool PrintMessage { get; set; } = true;
}
