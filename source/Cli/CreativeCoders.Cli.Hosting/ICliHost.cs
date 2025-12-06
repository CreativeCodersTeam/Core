namespace CreativeCoders.Cli.Hosting;

public interface ICliHost
{
    Task<CliResult> RunAsync(string[] args);
}
