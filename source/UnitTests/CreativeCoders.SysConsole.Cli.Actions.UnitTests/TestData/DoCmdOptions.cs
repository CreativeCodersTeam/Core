using CreativeCoders.SysConsole.Cli.Parsing;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Cli.Actions.UnitTests.TestData
{
    [PublicAPI]
    public class DoCmdOptions
    {
        [OptionParameter('t', "text")]
        public string Text { get; set; }
    }
}
