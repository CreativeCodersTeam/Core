namespace CreativeCoders.SysConsole.Cli.Parsing.UnitTests.TestData;

public class TestOptionWithValueOption
{
    [OptionValue(0, IsRequired = true)]
    public string? TestValue { get; set; }
}