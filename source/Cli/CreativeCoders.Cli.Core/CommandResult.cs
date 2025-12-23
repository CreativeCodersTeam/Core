using JetBrains.Annotations;

namespace CreativeCoders.Cli.Core;

[PublicAPI]
public class CommandResult
{
    public CommandResult() { }

    public CommandResult(int exitCode) : this()
    {
        ExitCode = exitCode;
    }

    public int ExitCode { get; init; }
}
