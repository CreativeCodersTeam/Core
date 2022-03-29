using System.Diagnostics.CodeAnalysis;
using CreativeCoders.SysConsole.Cli.Actions.Definition;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData;

[CliController]
[CliController("config")]
public class TestControllerWithDefault
{
    [CliAction]
    [CliAction("help")]
    public void ShowHelp() { }

    [CliAction("setup", HelpText = "Setups the config")]
    [SuppressMessage("Style", "IDE0060:Remove unused parameter")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    public void Setup(OptionsForHelp options) { }
}
