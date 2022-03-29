using CreativeCoders.SysConsole.Cli.Parsing;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.TestData;

public class TestCommandOptions
{
    [OptionParameter('t', "text")]
    public string? Text { get; [UsedImplicitly] set; }
}