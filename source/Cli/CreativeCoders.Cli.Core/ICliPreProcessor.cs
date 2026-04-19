using JetBrains.Annotations;

namespace CreativeCoders.Cli.Core;

/// <summary>
/// Defines a pre-processor that executes before a CLI command is processed.
/// </summary>
[PublicAPI]
public interface ICliPreProcessor
{
    /// <summary>
    /// Executes the pre-processor asynchronously with the provided command line arguments.
    /// </summary>
    /// <param name="args">The command line arguments passed to the CLI application.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ExecuteAsync(string[] args);

    /// <summary>
    /// Gets the condition that determines when this pre-processor is executed.
    /// </summary>
    /// <value>One of the enumeration values that specifies the execution condition.</value>
    CliProcessorExecutionCondition ExecutionCondition { get; }
}
