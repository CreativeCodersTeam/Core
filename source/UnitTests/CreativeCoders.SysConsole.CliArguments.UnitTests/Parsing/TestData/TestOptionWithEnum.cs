using CreativeCoders.SysConsole.Cli.Parsing;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Parsing.TestData
{
    [PublicAPI]
    public class TestOptionWithEnum
    {
        [OptionParameter('e', "enum")]
        public TestEnum EnumValue { get; set; }
    }
}
