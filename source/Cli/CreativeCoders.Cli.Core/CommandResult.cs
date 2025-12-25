using JetBrains.Annotations;

namespace CreativeCoders.Cli.Core;

[PublicAPI]
public class CommandResult
{
    public static CommandResult Success { get; } = new CommandResult();

    public CommandResult() { }

    public CommandResult(int exitCode)
    {
        ExitCode = exitCode;
    }

    public int ExitCode { get; init; }

    public static implicit operator CommandResult(int exitCode) => new CommandResult(exitCode);
}
