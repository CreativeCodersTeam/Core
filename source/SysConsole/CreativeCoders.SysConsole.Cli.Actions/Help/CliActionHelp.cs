using CreativeCoders.SysConsole.Cli.Parsing.Help;

namespace CreativeCoders.SysConsole.Cli.Actions.Help
{
    public class CliActionHelp
    {
        public CliActionHelp(string helpText, OptionsHelp optionsHelp)
        {
            HelpText = helpText;
            OptionsHelp = optionsHelp;
        }

        public string HelpText { get; }

        public OptionsHelp OptionsHelp { get; }
    }
}
