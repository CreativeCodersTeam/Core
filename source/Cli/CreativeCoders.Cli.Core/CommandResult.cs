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

    public CommandResult() { }

    public CommandResult(int exitCode)
    {
        ExitCode = exitCode;
    }

    public int ExitCode { get; init; }

    public static implicit operator CommandResult(int exitCode)
        => exitCode == 0
            ? Success
            : new CommandResult(exitCode);
}
