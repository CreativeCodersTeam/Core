using System.Collections.Generic;
using System.Collections.Immutable;

namespace CreativeCoders.SysConsole.Cli.Parsing.Help;

public class OptionsHelp
{
    public OptionsHelp(IEnumerable<HelpEntry> valueHelpEntries,
        IEnumerable<HelpEntry> parameterHelpEntries)
    {
        ValueHelpEntries = [..valueHelpEntries];
        ParameterHelpEntries = [..parameterHelpEntries];
    }

    public IImmutableList<HelpEntry> ValueHelpEntries { get; }

    public IImmutableList<HelpEntry> ParameterHelpEntries { get; }
}
