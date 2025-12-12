using JetBrains.Annotations;

namespace CreativeCoders.Cli.Hosting.Exceptions;

[PublicAPI]
public class CliCommandNotFoundException(string message, string[] args)
    : CliExitException(message, CliExitCodes.CommandNotFound)
{
    public string[] Args { get; } = args;
}
