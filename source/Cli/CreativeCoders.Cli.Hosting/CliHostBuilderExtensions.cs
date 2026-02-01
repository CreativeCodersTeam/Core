using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.PreProcessors;

namespace CreativeCoders.Cli.Hosting;

[ExcludeFromCodeCoverage]
public static class CliHostBuilderExtensions
{
    /// <summary>
    /// Registers a pre-processor to print a header as plain text. The header is displayed when the CLI application
    /// is executed, before the command processing, based on the specified execution condition.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ICliHostBuilder"/> instance to which the pre-processor is being added.
    /// </param>
    /// <param name="lines">
    /// An enumerable collection of strings representing the lines of text to be displayed in the header.
    /// Each string represents an individual line.
    /// </param>
    /// <param name="executionCondition">
    /// A value from <see cref="CliProcessorExecutionCondition"/> that determines when the header should be displayed.
    /// Defaults to <see cref="CliProcessorExecutionCondition.Always"/>.
    /// </param>
    /// <returns>
    /// The <see cref="ICliHostBuilder"/> instance for method chaining.
    /// </returns>
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

    /// <summary>
    /// Registers a pre-processor to print a header using markup text. The header is displayed during the CLI application
    /// execution, before the command processing, based on the specified execution condition.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ICliHostBuilder"/> instance to which the pre-processor is being added.
    /// </param>
    /// <param name="lines">
    /// An enumerable collection of strings representing the lines of markup text to be displayed in the header.
    /// The markup can include formatting instructions that are interpreted during rendering.
    /// </param>
    /// <param name="executionCondition">
    /// A value from <see cref="CliProcessorExecutionCondition"/> that determines when the header should be displayed.
    /// Defaults to <see cref="CliProcessorExecutionCondition.Always"/>.
    /// </param>
    /// <returns>
    /// The <see cref="ICliHostBuilder"/> instance for method chaining.
    /// </returns>
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

    /// <summary>
    /// Registers a post-processor to print a footer as plain text. The footer is displayed after the command processing
    /// is complete, based on the specified execution condition.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ICliHostBuilder"/> instance to which the post-processor is being added.
    /// </param>
    /// <param name="lines">
    /// An enumerable collection of strings representing the lines of text to be displayed in the footer.
    /// Each string represents an individual line.
    /// </param>
    /// <param name="executionCondition">
    /// A value from <see cref="CliProcessorExecutionCondition"/> that determines when the footer should be displayed.
    /// Defaults to <see cref="CliProcessorExecutionCondition.Always"/>.
    /// </param>
    /// <returns>
    /// The <see cref="ICliHostBuilder"/> instance for method chaining.
    /// </returns>
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

    /// <summary>
    /// Registers a post-processor to print a footer using markup formatting. The footer is displayed
    /// after the command execution, based on the specified execution condition.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ICliHostBuilder"/> instance to which the post-processor is being added.
    /// </param>
    /// <param name="lines">
    /// An enumerable collection of strings representing the lines of footer text to be displayed.
    /// Each string supports markup formatting for better visual representation.
    /// </param>
    /// <param name="executionCondition">
    /// A value from <see cref="CliProcessorExecutionCondition"/> that determines when the footer should be displayed.
    /// Defaults to <see cref="CliProcessorExecutionCondition.Always"/>.
    /// </param>
    /// <returns>
    /// The <see cref="ICliHostBuilder"/> instance for method chaining.
    /// </returns>
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
