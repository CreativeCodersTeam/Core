namespace CreativeCoders.Cli.Hosting.Exceptions;

public class CliCommandNotFoundException(string message, string[] args) : Exception(message)
{
    public string[] Args { get; } = args;
}
