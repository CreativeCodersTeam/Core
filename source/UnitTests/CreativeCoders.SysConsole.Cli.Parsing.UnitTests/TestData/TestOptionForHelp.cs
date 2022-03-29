namespace CreativeCoders.SysConsole.Cli.Parsing.UnitTests.TestData;

public class TestOptionForHelp
{
    public const char TitleShortName = 't';

    public const string TitleLongName = "title";

    public const string TitleName = "Title";

    public const string TitleHelpText = "The title for the operation";

    public const string FirstValueHelpText = "This is the first value";

    [OptionParameter(TitleShortName, TitleLongName, Name = TitleName, HelpText = TitleHelpText)]
    public string? Title { get; set; }

    [OptionParameter('v', "verbose")]
    public bool Verbose { get; set; }

    [OptionValue(0, HelpText = FirstValueHelpText)]
    public string? FirstValue { get; set; }
}