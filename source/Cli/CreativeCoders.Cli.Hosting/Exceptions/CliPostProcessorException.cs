using CreativeCoders.Cli.Core;

namespace CreativeCoders.Cli.Hosting.Exceptions;

/// <summary>
/// Represents an exception thrown when a CLI post-processor fails during execution.
/// </summary>
/// <param name="postProcessor">The post-processor that caused the failure.</param>
/// <param name="message">The error message.</param>
/// <param name="innerException">The inner exception that caused the failure.</param>
public class CliPostProcessorException(
    ICliPostProcessor postProcessor,
    string message,
    Exception? innerException) : CliExitException(message, CliExitCodes.PostProcessorFailed, innerException)
{
    /// <summary>
    /// Gets the post-processor that caused the failure.
    /// </summary>
    /// <value>The <see cref="ICliPostProcessor"/> instance that failed.</value>
    public ICliPostProcessor PostProcessor { get; } = postProcessor;
}
