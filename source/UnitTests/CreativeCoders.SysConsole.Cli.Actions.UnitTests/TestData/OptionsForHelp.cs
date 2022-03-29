using CreativeCoders.SysConsole.Cli.Parsing;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData;

public class OptionsForHelp
{
    [OptionValue(0, HelpText = "Options text")]
    public string? Text { get; set; }
}