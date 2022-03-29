using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Parsing.UnitTests.TestData;

[PublicAPI]
public class TestOptionWithEnum
{
    [OptionParameter('e', "enum")]
    public TestEnum EnumValue { get; set; }
}