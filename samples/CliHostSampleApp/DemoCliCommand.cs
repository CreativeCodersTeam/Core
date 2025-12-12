using System.Diagnostics;
using System.Reflection;
using CreativeCoders.Cli.Core;
using CreativeCoders.SysConsole.Cli.Parsing;
using JetBrains.Annotations;

[assembly: CliCommandGroup(["demo"], "Demo commands root group")]
[assembly: CliCommandGroup(["demo", "do"], "Demo do commands")]

namespace CliHostSampleApp;

[CliCommand(["demo", "do", "list"]
    , AlternativeCommands = ["demo1"])]
//)]
public class DemoCliCommand : ICliCommand
{
    public DemoCliCommand() { }

    public async Task<CommandResult> ExecuteAsync()
    {
        Console.WriteLine("Hello World from cli command !");

        return new CommandResult();
    }
}

[CliCommand(["demo", "do", "something"], Name = "Demo2 command",
    //AlternativeCommands = ["demo", "do", "list"],
    Description = "Simple Demo command with options, that prints some text from options.")]
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
    [OptionValue(0, HelpText = "Name of the demo")]
    public string? Name { get; set; }

    [OptionParameter('t', "text", HelpText = "Some text", DefaultValue = "Default Text")]
    public string? Text { get; set; }
}
