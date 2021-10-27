using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Parsing.TestData
{
    public class TestOptionForParser
    {
        [OptionValue(0)]
        public string HelloWorld { get; set; }

        [OptionParameter('t', "text")]
        public string TextValue { get; set; }
    }
}
