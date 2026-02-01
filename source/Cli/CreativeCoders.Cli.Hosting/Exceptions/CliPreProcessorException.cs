using CreativeCoders.Cli.Core;

namespace CreativeCoders.Cli.Hosting.Exceptions;

public class CliPreProcessorException(
    ICliPreProcessor preProcessor,
    string message,
    Exception? innerException)
    : CliExitException(message, CliExitCodes.PreProcessorFailed, innerException)
{
    public ICliPreProcessor PreProcessor { get; } = preProcessor;
}
