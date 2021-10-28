using CreativeCoders.SysConsole.CliArguments.Options;
using CreativeCoders.SysConsole.CliArguments.UnitTests.TestData;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Parsing.TestData
{
    public class TestOptionWithEnum
    {
        [OptionParameter('e', "enum")]
        public TestEnum EnumValue { get; set; }
    }
}
