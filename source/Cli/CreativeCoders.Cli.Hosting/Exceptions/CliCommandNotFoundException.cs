using JetBrains.Annotations;

namespace CreativeCoders.Cli.Hosting.Exceptions;

/// <summary>
/// Represents an exception thrown when no CLI command matches the provided arguments.
/// </summary>
/// <param name="message">The error message.</param>
/// <param name="args">The command line arguments that did not match any command.</param>
[PublicAPI]
public class CliCommandNotFoundException(string message, string[] args)
    : CliExitException(message, CliExitCodes.CommandNotFound)
{
    /// <summary>
    /// Gets the command line arguments that did not match any registered command.
    /// </summary>
    /// <value>An array of unmatched argument strings.</value>
    public string[] Args { get; } = args;
}
