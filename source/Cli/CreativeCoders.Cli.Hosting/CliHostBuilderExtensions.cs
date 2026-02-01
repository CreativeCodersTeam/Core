using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.PreProcessors;

namespace CreativeCoders.Cli.Hosting;

public static class CliHostBuilderExtensions
{
    public static ICliHostBuilder PrintHeaderText(this ICliHostBuilder builder, IEnumerable<string> lines,
        PreProcessorExecutionCondition executionCondition = PreProcessorExecutionCondition.Always)
    {
        return builder.RegisterPreProcessor<PrintHeaderPreProcessor>(x =>
        {
            x.Lines = lines;
            x.PlainText = true;
            x.ExecutionCondition = executionCondition;
        });
    }

    public static ICliHostBuilder PrintHeaderMarkup(this ICliHostBuilder builder, IEnumerable<string> lines,
        PreProcessorExecutionCondition executionCondition = PreProcessorExecutionCondition.Always)
    {
        return builder.RegisterPreProcessor<PrintHeaderPreProcessor>(x =>
        {
            x.Lines = lines;
            x.PlainText = false;
            x.ExecutionCondition = executionCondition;
        });
    }
}
