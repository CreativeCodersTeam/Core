namespace CreativeCoders.Cli.Hosting.Exceptions;

public class CliCommandNotFoundException(string message, string[] args)
    : CliExitException(message, CliExitCodes.CommandNotFound)
{
    public string[] Args { get; } = args;
}
