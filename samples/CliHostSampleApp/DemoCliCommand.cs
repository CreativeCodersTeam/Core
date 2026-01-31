using CreativeCoders.Cli.Core;
using JetBrains.Annotations;

namespace CliHostSampleApp;

[UsedImplicitly]
[CliCommand([DemoCommandGroup.Name, "do"]
    , AlternativeCommands = ["demo1"])]
public class DemoCliCommand : ICliCommand
{
    public Task<CommandResult> ExecuteAsync()
    {
        Console.WriteLine("Hello World from cli command !");

        return Task.FromResult(new CommandResult());
    }
}
