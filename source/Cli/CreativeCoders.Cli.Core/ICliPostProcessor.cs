namespace CreativeCoders.Cli.Core;

public interface ICliPostProcessor
{
    Task ExecuteAsync(CliResult cliResult);
}
