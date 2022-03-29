using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Parsing.UnitTests.TestData;

public class TestOptionWithBool
{
    [OptionParameter('v', "verbose")] public bool Verbose { get; [UsedImplicitly] set; }

    [OptionParameter('b', "bold")] public bool Bold { get; [UsedImplicitly] set; }
}
