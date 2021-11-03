using CreativeCoders.SysConsole.Cli.Parsing;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Parsing.TestData
{
    [PublicAPI]
    public class TestOptionWithEnumFlags
    {
        [OptionParameter('e', "enums")]
        public TestEnumWithFlags EnumValue { get; set; }
    }
}
