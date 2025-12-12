using System.Collections.Immutable;
using CreativeCoders.Cli.Core;
using CreativeCoders.Cli.Hosting.Commands;
using CreativeCoders.Cli.Hosting.Commands.Store;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.SysConsole.Cli.Parsing.Help;
using CreativeCoders.SysConsole.Core;
using Spectre.Console;

namespace CreativeCoders.Cli.Hosting.Help;

public class CliCommandHelpHandler(
    HelpHandlerSettings settings,
    ICliCommandStore commandStore,
    IAnsiConsole ansiConsole,
    IOptionsHelpGenerator optionsHelpGenerator)
    : ICliCommandHelpHandler
{
    private readonly IOptionsHelpGenerator _optionsHelpGenerator = Ensure.NotNull(optionsHelpGenerator);

    private readonly IAnsiConsole _ansiConsole = Ensure.NotNull(ansiConsole);

    private readonly ICliCommandStore _commandStore = Ensure.NotNull(commandStore);

    private readonly HelpHandlerSettings _settings = Ensure.NotNull(settings);

    public bool ShouldPrintHelp(string[] args)
    {
        var lowerCaseArgs = args.Select(x => x.ToLower()).ToArray();

        return _settings.CommandKind switch
        {
            HelpCommandKind.Command => lowerCaseArgs.FirstOrDefault() == "help",
            HelpCommandKind.Argument => lowerCaseArgs.Contains("--help"),
            HelpCommandKind.CommandOrArgument => lowerCaseArgs.FirstOrDefault() == "help" ||
                                                 lowerCaseArgs.Contains("--help"),
            _ => false
        };
    }

    public void PrintHelp(string[] args)
    {
        if (args.FirstOrDefault()?.ToLower() == "help")
        {
            args = args.Skip(1).ToArray();
        }

        var findCommandNodeResult = _commandStore.FindCommandNode(args);

        if (findCommandNodeResult?.Node != null)
        {
            PrintHelpFor(findCommandNodeResult.Node.CommandInfo);

            return;
        }

        var findCommandGroupNodeResult = _commandStore.FindCommandGroupNode(args);

        if (findCommandGroupNodeResult?.Node != null)
        {
            PrintHelpFor(findCommandGroupNodeResult.Node.ChildNodes);

            return;
        }

        PrintHelpFor(_commandStore.TreeRootNodes.ToList());
    }

    public void PrintHelpFor(IList<CliTreeNode> nodeChildNodes)
    {
        if (nodeChildNodes.Count == 0)
        {
            return;
        }

        var groups = nodeChildNodes.OfType<CliCommandGroupNode>().ToArray();

        if (groups.Length != 0)
        {
            _ansiConsole.WriteLine();
            _ansiConsole.WriteLine("Groups:");

            _ansiConsole.Write(CreateTableFor(
                groups
                    .Select(x =>
                        new KeyValuePair<string?, string?>(x.Name,
                            x.GroupAttribute?.Description ?? string.Empty))));
        }

        var commands = nodeChildNodes.OfType<CliCommandNode>().ToArray();

        if (commands.Length == 0)
        {
            return;
        }

        _ansiConsole.WriteLine();
        _ansiConsole.WriteLine("Commands:");

        _ansiConsole.Write(CreateTableFor(commands.Select(x =>
            new KeyValuePair<string?, string?>(x.Name, x.CommandInfo.CommandAttribute.Description))));
    }

    private void PrintHelpFor(CliCommandInfo commandInfo)
    {
        var description = commandInfo.CommandAttribute.Description;

        _ansiConsole.MarkupLine($"[bold]{commandInfo.CommandAttribute.Name}[/]");

        if (!string.IsNullOrWhiteSpace(description))
        {
            _ansiConsole.PrintBlock()
                .WriteLine()
                .WriteLine("Description:")
                .Write("  ")
                .WriteLine(commandInfo.CommandAttribute.Description);
        }

        var optionsHelp = commandInfo.OptionsType == null
            ? null
            : _optionsHelpGenerator.CreateHelp(commandInfo.OptionsType);

        PrintSyntax(commandInfo.CommandAttribute, optionsHelp);

        if (optionsHelp != null)
        {
            PrintOptionsHelp(optionsHelp);
        }

        _ansiConsole.WriteLine();
    }

    private void PrintSyntax(CliCommandAttribute commandAttribute, OptionsHelp? optionsHelp)
    {
        var commandSyntax = string.Join(" ", commandAttribute.Commands);

        if (optionsHelp != null)
        {
            var values = string.Join(" ", optionsHelp.ValueHelpEntries.Select(x => $"<{x.ArgumentName}>"));

            if (!string.IsNullOrWhiteSpace(values))
            {
                commandSyntax += $" [[{values}]]";
            }

            if (optionsHelp.ParameterHelpEntries.Count != 0)
            {
                commandSyntax += " [[options]]";
            }
        }

        _ansiConsole.PrintBlock()
            .WriteLine()
            .WriteLine("Syntax:")
            .MarkupLine($"  [bold]{commandSyntax}[/]");
    }

    private void PrintOptionsHelp(OptionsHelp optionsHelp)
    {
        PrintOptionsValuesHelp(optionsHelp.ValueHelpEntries);
        PrintOptionsParametersHelp(optionsHelp.ParameterHelpEntries);
    }

    private void PrintOptionsValuesHelp(IImmutableList<HelpEntry> valueHelpEntries)
    {
        if (valueHelpEntries.Count == 0)
        {
            return;
        }

        _ansiConsole.PrintBlock()
            .WriteLine()
            .WriteLine("Arguments:");

        _ansiConsole.Write(CreateTableForHelpEntries(valueHelpEntries, "<{0}>"));
    }

    private void PrintOptionsParametersHelp(IImmutableList<HelpEntry> parameterHelpEntries)
    {
        if (parameterHelpEntries.Count == 0)
        {
            return;
        }

        _ansiConsole.PrintBlock()
            .WriteLine()
            .WriteLine("Options:");

        _ansiConsole.Write(CreateTableForHelpEntries(parameterHelpEntries));
    }

    private static Table CreateTableForHelpEntries(IImmutableList<HelpEntry> helpEntries,
        string nameFormat = "{0}")
    {
        return CreateTableFor(
            helpEntries.Select(x => new KeyValuePair<string?, string?>(x.ArgumentName, x.HelpText)),
            nameFormat);
    }

    private static Table CreateTableFor(IEnumerable<KeyValuePair<string?, string?>> entries,
        string keyFormat = "{0}")
    {
        var table = new Table
        {
            ShowHeaders = false,
            Border = TableBorder.None
        };
        table
            .AddColumn("Indent", x => x.Width = 1)
            .AddColumn("Key")
            .AddColumn("Spacer", x => x.Width = 1)
            .AddColumn("Value");

        foreach (var entry in entries)
        {
            var name = string.Format(keyFormat, entry.Key ?? string.Empty);
            table.AddRow("", name, "", entry.Value ?? string.Empty);
        }

        return table;
    }
}
