namespace CreativeCoders.Cli.Hosting.Exceptions;

public class CliCommandAbortException(string message, int exitCode, Exception? exception = null)
    : CliExitException(message, exitCode, exception)
{
    public bool IsError { get; set; } = true;

    public bool PrintMessage { get; set; } = true;
}
