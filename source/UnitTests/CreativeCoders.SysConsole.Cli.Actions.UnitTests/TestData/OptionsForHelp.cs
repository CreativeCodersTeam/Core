using CreativeCoders.SysConsole.Cli.Parsing;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData;

[PublicAPI]
public class OptionsForHelp
{
    [OptionValue(0, HelpText = "Options text")]
    public string? Text { get; set; }
}
