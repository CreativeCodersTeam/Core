using CreativeCoders.Cli.Core;
using CreativeCoders.SysConsole.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CliHostSampleApp;

[UsedImplicitly]
[CliCommand([DemoCommandGroup.Name, "do"]
    , AlternativeCommands = ["demo1"])]
public class DemoCliCommand(IAnsiConsole ansiConsole) : ICliCommand
{
    public Task<CommandResult> ExecuteAsync()
    {
        Console.WriteLine("Hello World from cli command !");

        ansiConsole.WriteLines("Test", 1234.ToString());

        return Task.FromResult(new CommandResult());
    }
}
