using System.Collections.Generic;

namespace CreativeCoders.SysConsole.Cli.Parsing.Help
{
    public class OptionsHelp
    {
        public OptionsHelp(IEnumerable<HelpEntry> valueHelpEntries,
            IEnumerable<HelpEntry> parameterHelpEntries)
        {
            ValueHelpEntries = valueHelpEntries;
            ParameterHelpEntries = parameterHelpEntries;
        }

        public IEnumerable<HelpEntry> ValueHelpEntries { get; }

        public IEnumerable<HelpEntry> ParameterHelpEntries { get; }
    }
}
