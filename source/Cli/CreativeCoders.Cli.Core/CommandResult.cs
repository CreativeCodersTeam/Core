using JetBrains.Annotations;

namespace CreativeCoders.Cli.Core;

/// <summary>
/// Represents the result of executing a command.
/// </summary>
[PublicAPI]
public class CommandResult
{
    /// <summary>
    /// Represents a successful result of a command execution with an implicit default exit code of 0.
    /// </summary>
    public static CommandResult Success { get; } = new CommandResult();

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandResult"/> class with an exit code of 0.
    /// </summary>
    public CommandResult() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandResult"/> class with the specified exit code.
    /// </summary>
    /// <param name="exitCode">The exit code for the command result.</param>
    public CommandResult(int exitCode)
    {
        ExitCode = exitCode;
    }

    /// <summary>
    /// Gets the exit code of the command execution.
    /// </summary>
    /// <value>The exit code. The default is 0.</value>
    public int ExitCode { get; init; }

    /// <summary>
    /// Implicitly converts an integer exit code to a <see cref="CommandResult"/>.
    /// </summary>
    /// <param name="exitCode">The exit code to convert.</param>
    /// <returns>A <see cref="CommandResult"/> representing the exit code. Returns <see cref="Success"/> if the exit code is 0.</returns>
    public static implicit operator CommandResult(int exitCode)
        => exitCode == 0
            ? Success
            : new CommandResult(exitCode);
}
