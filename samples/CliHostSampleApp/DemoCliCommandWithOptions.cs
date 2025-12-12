using CreativeCoders.Cli.Core;
using JetBrains.Annotations;

namespace CliHostSampleApp;

[UsedImplicitly]
[CliCommand(["demo", "do", "something"], Name = "Demo2 command",
    Description = "Simple Demo command with options, that prints some text from options.")]
public class DemoCliCommandWithOptions : ICliCommand<DemoOptions>
{
    public Task<CommandResult> ExecuteAsync(DemoOptions options)
    {
        Console.WriteLine("Hello World from cli command with options !");
        Console.WriteLine($"Text: {options.Text}");

        return Task.FromResult(new CommandResult());
    }
}