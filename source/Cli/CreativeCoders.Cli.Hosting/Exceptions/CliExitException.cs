namespace CreativeCoders.Cli.Hosting.Exceptions;

/// <summary>
/// Represents a CLI exception that causes the application to exit with a specific exit code.
/// </summary>
/// <param name="message">The error message.</param>
/// <param name="exitCode">The exit code to return.</param>
/// <param name="exception">The optional inner exception.</param>
public class CliExitException(string message, int exitCode, Exception? exception = null)
    : Exception(message, exception)
{
    /// <summary>
    /// Gets the exit code associated with this exception.
    /// </summary>
    /// <value>The exit code.</value>
    public int ExitCode { get; } = exitCode;
}
