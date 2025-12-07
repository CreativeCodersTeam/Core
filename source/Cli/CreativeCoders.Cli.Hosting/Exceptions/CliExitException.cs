namespace CreativeCoders.Cli.Hosting.Exceptions;

public class CliExitException(string message, int exitCode, Exception? exception = null)
    : Exception(message, exception)
{
    public int ExitCode { get; } = exitCode;
}
