using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.SysConsole.Cli.Parsing.Help;
using CreativeCoders.SysConsole.Core.Abstractions;

namespace CreativeCoders.SysConsole.Cli.Actions.Help;

[ExcludeFromCodeCoverage]
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
            .WriteLine("Syntax:")
            .WriteLine($"  {help.Syntax}");

        PrintHelpEntries(help.OptionsHelp.ValueHelpEntries, "Arguments:");

        PrintHelpEntries(help.OptionsHelp.ParameterHelpEntries, "Options:");

        _sysConsole.WriteLine();
    }

    private void PrintHelpEntries(IImmutableList<HelpEntry> helpEntries, string entriesHeader)
    {
        if (!helpEntries.Any())
        {
            return;
        }

        _sysConsole
            .WriteLine()
            .WriteLine(entriesHeader)
            .WriteLine();

        var firstColumnWidth = helpEntries.Select(x => x.ArgumentName?.Length ?? 0).Max() + 3;

        helpEntries
            .ForEach(x =>
                _sysConsole.WriteLine($"  {x.ArgumentName?.PadRight(firstColumnWidth)}{x.HelpText}"));
    }
}
