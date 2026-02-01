using JetBrains.Annotations;

namespace CreativeCoders.Cli.Core;

[PublicAPI]
public interface ICliPreProcessor
{
    Task ExecuteAsync(string[] args);

    CliProcessorExecutionCondition ExecutionCondition { get; }
}
