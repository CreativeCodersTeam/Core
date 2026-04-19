using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Cli.Core;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.Cli.Hosting.Processors;

/// <summary>
/// Prints header lines to the console before command execution as a CLI pre-processor.
/// </summary>
/// <param name="ansiConsole">The console used for rendering output.</param>
[UsedImplicitly]
[ExcludeFromCodeCoverage]
public class PrintHeaderPreProcessor(IAnsiConsole ansiConsole) : ICliPreProcessor
{
    private readonly IAnsiConsole _ansiConsole = Ensure.NotNull(ansiConsole);

    /// <inheritdoc />
    public Task ExecuteAsync(string[] args)
    {
        if (PlainText)
        {
            foreach (var line in Lines)
            {
                _ansiConsole.WriteLine(line);
            }

            return Task.CompletedTask;
        }

        foreach (var line in Lines)
        {
            _ansiConsole.MarkupLine(line);
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public CliProcessorExecutionCondition ExecutionCondition { get; set; }

    /// <summary>
    /// Gets or sets the lines of text to display in the header.
    /// </summary>
    /// <value>An enumerable collection of header lines. The default is an empty collection.</value>
    public IEnumerable<string> Lines { get; set; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether the lines are rendered as plain text.
    /// </summary>
    /// <value><see langword="true"/> if the lines are rendered as plain text; otherwise, <see langword="false"/> to render as markup. The default is <see langword="false"/>.</value>
    public bool PlainText { get; set; }
}
