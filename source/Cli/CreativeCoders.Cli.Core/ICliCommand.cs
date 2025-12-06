namespace CreativeCoders.Cli.Core;

public interface ICliCommand<TOptions>
    where TOptions : class
{
    Task<CommandResult> ExecuteAsync(TOptions options);
}

public interface ICliCommand
{
    Task<CommandResult> ExecuteAsync();
}
