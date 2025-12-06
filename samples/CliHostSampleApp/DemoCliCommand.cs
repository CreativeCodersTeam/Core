using CreativeCoders.Cli.Core;

namespace CliHostSampleApp;

public class DemoCliCommand : ICliCommand<ICliCommandContext>
{
    public DemoCliCommand() { }

    public async Task<CommandResult> ExecuteAsync(ICliCommandContext context)
    {
        Console.WriteLine("Hello World!");

        return new CommandResult();
    }
}
