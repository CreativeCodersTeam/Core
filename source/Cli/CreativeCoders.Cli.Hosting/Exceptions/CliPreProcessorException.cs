using CreativeCoders.Cli.Core;

namespace CreativeCoders.Cli.Hosting.Exceptions;

/// <summary>
/// Represents an exception thrown when a CLI pre-processor fails during execution.
/// </summary>
/// <param name="preProcessor">The pre-processor that caused the failure.</param>
/// <param name="message">The error message.</param>
/// <param name="innerException">The inner exception that caused the failure.</param>
public class CliPreProcessorException(
    ICliPreProcessor preProcessor,
    string message,
    Exception? innerException)
    : CliExitException(message, CliExitCodes.PreProcessorFailed, innerException)
{
    /// <summary>
    /// Gets the pre-processor that caused the failure.
    /// </summary>
    /// <value>The <see cref="ICliPreProcessor"/> instance that failed.</value>
    public ICliPreProcessor PreProcessor { get; } = preProcessor;
}
