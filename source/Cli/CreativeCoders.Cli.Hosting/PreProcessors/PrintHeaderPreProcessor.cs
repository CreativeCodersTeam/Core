using CreativeCoders.Cli.Core;
using CreativeCoders.Core;
using Spectre.Console;

namespace CreativeCoders.Cli.Hosting.PreProcessors;

public class PrintHeaderPreProcessor(IAnsiConsole ansiConsole) : ICliPreProcessor
{
    private readonly IAnsiConsole _ansiConsole = Ensure.NotNull(ansiConsole);

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

    public PreProcessorExecutionCondition ExecutionCondition { get; set; }

    public IEnumerable<string> Lines { get; set; } = [];

    public bool PlainText { get; set; }
}
