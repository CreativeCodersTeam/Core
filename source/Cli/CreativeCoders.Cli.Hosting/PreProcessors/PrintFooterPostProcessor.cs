using CreativeCoders.Cli.Core;
using CreativeCoders.Core;
using Spectre.Console;

namespace CreativeCoders.Cli.Hosting.PreProcessors;

public class PrintFooterPostProcessor(IAnsiConsole ansiConsole) : ICliPostProcessor
{
    private readonly IAnsiConsole _ansiConsole = Ensure.NotNull(ansiConsole);

    public Task ExecuteAsync(CliResult cliResult)
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

    public CliProcessorExecutionCondition ExecutionCondition { get; set; }

    public IEnumerable<string> Lines { get; set; } = [];

    public bool PlainText { get; set; }
}
