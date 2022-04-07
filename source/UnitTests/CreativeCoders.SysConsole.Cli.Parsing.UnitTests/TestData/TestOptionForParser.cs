using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Parsing.UnitTests.TestData;

public class TestOptionForParser
{
    [OptionValue(0)] public string? HelloWorld { get; [UsedImplicitly] set; }

    [OptionParameter('t', "text")] public string? TextValue { get; [UsedImplicitly] set; }
}
