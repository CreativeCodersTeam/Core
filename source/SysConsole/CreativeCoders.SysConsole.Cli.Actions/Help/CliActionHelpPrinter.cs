using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.SysConsole.Cli.Parsing.Help;
using CreativeCoders.SysConsole.Core.Abstractions;

namespace CreativeCoders.SysConsole.Cli.Actions.Help
{
    public class CliActionHelpPrinter : ICliActionHelpPrinter
    {
        private readonly ISysConsole _sysConsole;

        private readonly ICliActionHelpGenerator _helpGenerator;

        public CliActionHelpPrinter(ICliActionHelpGenerator helpGenerator, ISysConsole sysConsole)
        {
            _sysConsole = Ensure.NotNull(sysConsole, nameof(sysConsole));
            _helpGenerator = Ensure.NotNull(helpGenerator, nameof(helpGenerator));
        }

        public void PrintHelp(IEnumerable<string> actionRouteParts)
        {
            var help = _helpGenerator.CreateHelp(actionRouteParts);

            _sysConsole
                .WriteLine()
                .WriteLine(help.HelpText)
                .WriteLine()
                .WriteLine("Arguments:")
                .WriteLine();

            PrintHelpEntries(help.OptionsHelp.ValueHelpEntries);

            _sysConsole
                .WriteLine()
                .WriteLine("Parameters:")
                .WriteLine();

            PrintHelpEntries(help.OptionsHelp.ParameterHelpEntries);

            _sysConsole.WriteLine();
        }

        private void PrintHelpEntries(IImmutableList<HelpEntry> helpEntries)
        {
            var firstColumnWidth = helpEntries.Select(x => x.ArgumentName?.Length ?? 0).Max() + 3;

            helpEntries
                .ForEach(x =>
                    _sysConsole.WriteLine($" {x.ArgumentName?.PadRight(firstColumnWidth)}{x.HelpText}"));
        }
    }
}
