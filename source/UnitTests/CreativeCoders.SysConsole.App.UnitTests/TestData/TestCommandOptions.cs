using CreativeCoders.SysConsole.Cli.Parsing;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData;

[PublicAPI]
public class TestCommandOptions
{
    [OptionValue(0)] public string? FirstArg { get; set; }
}
