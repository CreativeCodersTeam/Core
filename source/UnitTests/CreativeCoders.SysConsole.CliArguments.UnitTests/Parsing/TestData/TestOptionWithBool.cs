using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Parsing
{
    public class TestOptionWithBool
    {
        [OptionParameter('v', "verbose")]
        public bool Verbose { get; set; }

        [OptionParameter('b', "bold")]
        public bool Bold { get; set; }
    }
}