using CreativeCoders.Cli.Core;

namespace CliHostSampleApp;

[CliCommand(["demo"])]
public class DemoCliCommand : ICliCommand
{
    public DemoCliCommand() { }

    public async Task<CommandResult> ExecuteAsync()
    {
        Console.WriteLine("Hello World from cli command !");

        return new CommandResult();
    }
}
