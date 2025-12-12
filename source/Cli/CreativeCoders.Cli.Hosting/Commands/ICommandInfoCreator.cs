namespace CreativeCoders.Cli.Hosting.Commands;

public interface ICommandInfoCreator
{
    CliCommandInfo? Create(Type commandType);
}