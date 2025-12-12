using JetBrains.Annotations;

namespace CreativeCoders.Cli.Hosting;

[PublicAPI]
public class CliResult(int exitCode)
{
    public int ExitCode { get; set; } = exitCode;
}
