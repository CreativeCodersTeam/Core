using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.PreProcessors;

namespace CreativeCoders.Cli.Hosting;

[ExcludeFromCodeCoverage]
public static class CliHostBuilderExtensions
{
    public static ICliHostBuilder PrintHeaderText(this ICliHostBuilder builder, IEnumerable<string> lines,
        CliProcessorExecutionCondition executionCondition = CliProcessorExecutionCondition.Always)
    {
        return builder.RegisterPreProcessor<PrintHeaderPreProcessor>(x =>
        {
            x.Lines = lines;
            x.PlainText = true;
            x.ExecutionCondition = executionCondition;
        });
    }

    public static ICliHostBuilder PrintHeaderMarkup(this ICliHostBuilder builder, IEnumerable<string> lines,
        CliProcessorExecutionCondition executionCondition = CliProcessorExecutionCondition.Always)
    {
        return builder.RegisterPreProcessor<PrintHeaderPreProcessor>(x =>
        {
            x.Lines = lines;
            x.PlainText = false;
            x.ExecutionCondition = executionCondition;
        });
    }

    public static ICliHostBuilder PrintFooterText(this ICliHostBuilder builder, IEnumerable<string> lines,
        CliProcessorExecutionCondition executionCondition = CliProcessorExecutionCondition.Always)
    {
        return builder.RegisterPostProcessor<PrintFooterPostProcessor>(x =>
        {
            x.Lines = lines;
            x.PlainText = true;
            x.ExecutionCondition = executionCondition;
        });
    }

    public static ICliHostBuilder PrintFooterMarkup(this ICliHostBuilder builder, IEnumerable<string> lines,
        CliProcessorExecutionCondition executionCondition = CliProcessorExecutionCondition.Always)
    {
        return builder.RegisterPostProcessor<PrintFooterPostProcessor>(x =>
        {
            x.Lines = lines;
            x.PlainText = false;
            x.ExecutionCondition = executionCondition;
        });
    }
}
