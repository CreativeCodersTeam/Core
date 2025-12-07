namespace CreativeCoders.Cli.Hosting.Exceptions;

public class CliCommandConstructionFailedException(string message, string[] args) : Exception(message)
{
    public string[] Args { get; } = args;
}
