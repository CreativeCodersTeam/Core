namespace CreativeCoders.Cli.Hosting;

public class CliResult(int exitCode)
{
    public int ExitCode { get; set; } = exitCode;
}
