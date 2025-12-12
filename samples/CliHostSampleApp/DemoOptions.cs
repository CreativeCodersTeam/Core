using CreativeCoders.SysConsole.Cli.Parsing;
using JetBrains.Annotations;

namespace CliHostSampleApp;

[UsedImplicitly]
public class DemoOptions
{
    [OptionValue(0, HelpText = "Name of the demo")]
    public string? Name { get; set; }

    [OptionParameter('t', "text", HelpText = "Some text", DefaultValue = "Default Text")]
    public string? Text { get; set; }
}