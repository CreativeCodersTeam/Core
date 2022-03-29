using CreativeCoders.SysConsole.Cli.Parsing.Help;

namespace CreativeCoders.SysConsole.Cli.Actions.Help;

public class CliActionHelp
{
    public CliActionHelp(OptionsHelp optionsHelp)
    {
        OptionsHelp = optionsHelp;
    }

    public string HelpText { get; init; } = string.Empty;

    public string Syntax { get; init; } = string.Empty;

    public OptionsHelp OptionsHelp { get; }
}
