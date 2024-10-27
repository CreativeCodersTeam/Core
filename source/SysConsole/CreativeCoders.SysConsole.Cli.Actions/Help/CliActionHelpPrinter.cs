using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.SysConsole.Cli.Parsing.Help;
using CreativeCoders.SysConsole.Core;
using Spectre.Console;

namespace CreativeCoders.SysConsole.Cli.Actions.Help;

[ExcludeFromCodeCoverage]
public class CliActionHelpPrinter(ICliActionHelpGenerator helpGenerator, IAnsiConsole sysConsole)
    : ICliActionHelpPrinter
{
    private readonly IAnsiConsole _ansiConsole = Ensure.NotNull(sysConsole);

    private readonly ICliActionHelpGenerator _helpGenerator = Ensure.NotNull(helpGenerator);

    private void PrintHelpEntries(IImmutableList<HelpEntry> helpEntries, string entriesHeader)
    {
        if (!helpEntries.Any())
        {
            return;
        }

        _ansiConsole.PrintBlock()
            .WriteLine()
            .WriteLine(entriesHeader)
            .WriteLine();

        var firstColumnWidth = helpEntries.Select(x => x.ArgumentName?.Length ?? 0).Max() + 3;

        var table = new Table()
            .Border(TableBorder.None)
            .HideHeaders()
            .AddColumn("Name", x => { x.Width = firstColumnWidth; })
            .AddColumn("Description");

        helpEntries
            .ForEach(x =>
                table.AddRow($"  {x.ArgumentName ?? string.Empty}", x.HelpText ?? string.Empty));

        _ansiConsole.Write(table);
    }

    public void PrintHelp(IEnumerable<string> actionRouteParts)
    {
        var help = _helpGenerator.CreateHelp(actionRouteParts);

        _ansiConsole.PrintBlock()
            .WriteLine()
            .WriteLine()
            .WriteLine(help.HelpText)
            .WriteLine()
            .WriteLine("Usage:")
            .WriteLine($"  {help.Syntax}");

        PrintHelpEntries(help.OptionsHelp.ValueHelpEntries, "Arguments:");

        PrintHelpEntries(help.OptionsHelp.ParameterHelpEntries, "Options:");

        _ansiConsole.WriteLine();
    }
}
