using CreativeCoders.Cli.Core;
using CreativeCoders.SysConsole.Cli.Parsing;
using JetBrains.Annotations;

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

[CliCommand(["demo2"])]
public class DemoCliCommandWithOptions : ICliCommand<DemoOptions>
{
    public DemoCliCommandWithOptions() { }

    public async Task<CommandResult> ExecuteAsync(DemoOptions options)
    {
        Console.WriteLine("Hello World from cli command with options !");
        Console.WriteLine($"Text: {options.Text}");

        return new CommandResult();
    }
}

[UsedImplicitly]
public class DemoOptions
{
    [OptionParameter('t', "text", HelpText = "Some text", DefaultValue = "Default Text")]
    public string Text { get; set; }
}
