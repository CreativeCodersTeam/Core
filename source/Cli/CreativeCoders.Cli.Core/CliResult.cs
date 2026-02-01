using JetBrains.Annotations;

namespace CreativeCoders.Cli.Core;

[PublicAPI]
public class CliResult(int exitCode)
{
    public int ExitCode { get; set; } = exitCode;
}
