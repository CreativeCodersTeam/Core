using CreativeCoders.SysConsole.CliArguments.Options;

namespace CreativeCoders.SysConsole.CliArguments.UnitTests.Parsing.TestData
{
    public class TestOptionWithTwoValues
    {
        [OptionValue(0)]
        public string FirstValue { get; set; }

        [OptionValue(1, DefaultValue = "Fallback")]
        public string SecondValue { get; set; }
    }
}
