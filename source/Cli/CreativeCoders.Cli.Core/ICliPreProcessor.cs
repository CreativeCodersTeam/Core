namespace CreativeCoders.Cli.Core;

public interface ICliPreProcessor
{
    Task ExecuteAsync(string[] args);

    CliProcessorExecutionCondition ExecutionCondition { get; }
}
