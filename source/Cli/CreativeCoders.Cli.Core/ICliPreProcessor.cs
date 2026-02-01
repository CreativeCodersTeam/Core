namespace CreativeCoders.Cli.Core;

public interface ICliPreProcessor
{
    Task ExecuteAsync(string[] args);

    PreProcessorExecutionCondition ExecutionCondition { get; }
}
