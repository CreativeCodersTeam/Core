using CreativeCoders.SysConsole.Cli.Parsing;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData;

[PublicAPI]
public class OptionsForHelp
{
    [OptionValue(0, HelpText = "Options text")]
    public string? Text { get; set; }

    [OptionParameter('v', "verbose", HelpText = "Verbose output")]
    public bool Verbose { get; set; }

    [OptionParameter('p', "port", HelpText = "Port number")]
    public int Port { get; set; }
}
