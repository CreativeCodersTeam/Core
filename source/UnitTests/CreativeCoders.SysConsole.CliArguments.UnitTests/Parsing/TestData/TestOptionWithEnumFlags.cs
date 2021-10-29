using CreativeCoders.SysConsole.CliArguments.Options;
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
