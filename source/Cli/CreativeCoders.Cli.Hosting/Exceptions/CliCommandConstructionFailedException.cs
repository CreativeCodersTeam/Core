using JetBrains.Annotations;

namespace CreativeCoders.Cli.Hosting.Exceptions;

/// <summary>
/// Represents an exception thrown when a CLI command could not be constructed from the service provider.
/// </summary>
/// <param name="message">The error message.</param>
/// <param name="args">The command line arguments that were being processed.</param>
/// <param name="exception">The optional inner exception that caused the construction failure.</param>
[PublicAPI]
public class CliCommandConstructionFailedException(
    string message,
    string[] args,
    Exception? exception = null)
    : CliExitException(message, CliExitCodes.CommandCreationFailed, exception)
{
    /// <summary>
    /// Gets the command line arguments that were being processed when construction failed.
    /// </summary>
    /// <value>An array of argument strings.</value>
    public string[] Args { get; } = args;
}
