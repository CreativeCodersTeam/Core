namespace CreativeCoders.Cli.Hosting.Exceptions;

public class CliCommandConstructionFailedException(
    string message,
    string[] args,
    Exception? exception = null)
    : CliExitException(message, CliExitCodes.CommandCreationFailed, exception)
{
    public string[] Args { get; } = args;
}
