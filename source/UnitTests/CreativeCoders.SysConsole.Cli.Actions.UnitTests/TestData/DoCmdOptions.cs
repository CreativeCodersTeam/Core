using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData
{
    public class DoCmdOptions
    {
        [OptionParameter('t', "text")]
        public string Text { get; set; }
    }
}
