using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Parsing.TestData
{
    public class TestOptionWithValueOption
    {
        [OptionValue(0, IsRequired = true)]
        public string? TestValue { get; set; }
    }
}
