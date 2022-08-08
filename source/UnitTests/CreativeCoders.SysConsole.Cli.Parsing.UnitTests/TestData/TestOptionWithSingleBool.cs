namespace CreativeCoders.SysConsole.Cli.Parsing.UnitTests.TestData;

public class TestOptionWithSingleBool
{
    [OptionParameter('c', "create")]
    public bool CreateData { get; set; }
}
