using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.App.UnitTests.TestData
{
    public class TestCommandOptions
    {
        [OptionValue(0)]
        public string? FirstArg { get; set; }
    }
}
