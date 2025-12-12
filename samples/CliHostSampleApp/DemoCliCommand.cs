using CreativeCoders.Cli.Core;
using JetBrains.Annotations;

[assembly: CliCommandGroup(["demo"], "Demo commands root group")]
[assembly: CliCommandGroup(["demo", "do"], "Demo do commands")]

namespace CliHostSampleApp;

[UsedImplicitly]
[CliCommand(["demo", "do", "list"]
    , AlternativeCommands = ["demo1"])]
public class DemoCliCommand : ICliCommand
{
    public Task<CommandResult> ExecuteAsync()
    {
        Console.WriteLine("Hello World from cli command !");

        return Task.FromResult(new CommandResult());
    }
}
