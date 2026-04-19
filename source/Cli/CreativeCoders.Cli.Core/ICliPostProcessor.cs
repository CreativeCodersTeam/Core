using JetBrains.Annotations;

namespace CreativeCoders.Cli.Core;

/// <summary>
/// Defines a post-processor that executes after a CLI command has completed.
/// </summary>
[PublicAPI]
public interface ICliPostProcessor
{
    /// <summary>
    /// Executes the post-processor asynchronously with the result of the CLI command.
    /// </summary>
    /// <param name="cliResult">The result of the CLI command execution.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ExecuteAsync(CliResult cliResult);

    /// <summary>
    /// Gets the condition that determines when this post-processor is executed.
    /// </summary>
    /// <value>One of the enumeration values that specifies the execution condition.</value>
    CliProcessorExecutionCondition ExecutionCondition { get; }
}
