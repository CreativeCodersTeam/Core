using CreativeCoders.Cli.Core;

namespace CreativeCoders.Cli.Hosting.Exceptions;

public class CliPostProcessorException(
    ICliPostProcessor postProcessor,
    string message,
    Exception? innerException) : CliExitException(message, CliExitCodes.PostProcessorFailed, innerException)
{
    public ICliPostProcessor PostProcessor { get; } = postProcessor;
}
