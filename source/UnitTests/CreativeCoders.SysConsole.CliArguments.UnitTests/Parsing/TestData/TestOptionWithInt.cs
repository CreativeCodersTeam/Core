using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Parsing
{
    public class TestOptionWithInt
    {
        [OptionParameter('i', "integer", IsRequired = true)]
        public int IntValue { get; set; }

        [OptionParameter('j', "integer2")]
        public int IntValue2 { get; set; }

        [OptionParameter('d', "default", DefaultValue = 1357)]
        public int DefIntValue { get; set; }
    }
}