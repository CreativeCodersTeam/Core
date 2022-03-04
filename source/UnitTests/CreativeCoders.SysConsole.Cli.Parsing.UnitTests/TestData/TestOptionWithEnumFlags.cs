using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Parsing.UnitTests.TestData
{
    [PublicAPI]
    public class TestOptionWithEnumFlags
    {
        [OptionParameter('e', "enums")]
        public TestEnumWithFlags EnumValue { get; set; }
    }
}
