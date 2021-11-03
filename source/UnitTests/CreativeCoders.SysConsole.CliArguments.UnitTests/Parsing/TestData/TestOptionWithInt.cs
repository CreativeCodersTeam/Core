using CreativeCoders.SysConsole.Cli.Parsing;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Parsing.TestData
{
    public class TestOptionWithInt
    {
        [OptionParameter('i', "integer", IsRequired = true)]
        public int IntValue { get; set; }

        [OptionParameter('j', "integer2")]
        public int IntValue2 { get; [UsedImplicitly] set; }

        [OptionParameter('d', "default", DefaultValue = 1357)]
        public int DefIntValue { get; [UsedImplicitly] set; }
    }
}
