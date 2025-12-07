namespace CreativeCoders.Cli.Core;

public interface ICliCommand<in TOptions>
    where TOptions : class
{
    Task<CommandResult> ExecuteAsync(TOptions options);
}

public interface ICliCommand
{
    Task<CommandResult> ExecuteAsync();
}
