using JetBrains.Annotations;

namespace CreativeCoders.Cli.Core;

[PublicAPI]
public interface ICliPostProcessor
{
    Task ExecuteAsync(CliResult cliResult);

    CliProcessorExecutionCondition ExecutionCondition { get; }
}
