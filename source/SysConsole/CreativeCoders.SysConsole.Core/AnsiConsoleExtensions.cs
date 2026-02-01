using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.SysConsole.Core;

[PublicAPI]
public static class AnsiConsoleExtensions
{
    public static IAnsiConsolePrint PrintBlock(this IAnsiConsole ansiConsole, bool clearConsole = false)
    {
        Ensure.NotNull(ansiConsole);

        if (clearConsole)
        {
            ansiConsole.Clear();
        }

        return new AnsiConsolePrint(ansiConsole);
    }

    [PublicAPI]
    public static IAnsiConsole Write<T>(this IAnsiConsole ansiConsole, T value, Color foregroundColor,
        Color? backgroundColor = null)
    {
        var style = new Style(foreground: foregroundColor, background: backgroundColor);

        ansiConsole.Write(value.ToStringSafe(), style);

        return ansiConsole;
    }

    [PublicAPI]
    public static IAnsiConsole WriteLine<T>(this IAnsiConsole ansiConsole, T value, Color foregroundColor,
        Color? backgroundColor = null)
    {
        var style = new Style(foreground: foregroundColor, background: backgroundColor);

        ansiConsole.WriteLine(value.ToStringSafe(), style);

        return ansiConsole;
    }

    [PublicAPI]
    public static void PrintTable<T>(this IAnsiConsole ansiConsole, IEnumerable<T> items,
        TableColumnDef<T>[] columns, Action<Table>? configureTable = null)
    {
        var table = new Table
        {
            ShowHeaders = false,
            Border = TableBorder.None
        };

        foreach (var tableColumnDef in columns)
        {
            table.AddColumn(tableColumnDef.GetTitle(), x =>
            {
                x.Width = tableColumnDef.Width;
                tableColumnDef.ConfigureColumn(x);
            });

            if (!string.IsNullOrWhiteSpace(tableColumnDef.Title))
            {
                table.ShowHeaders = true;
            }
        }

        configureTable?.Invoke(table);

        if (table.ShowHeaders)
        {
            table.AddRow(columns.Select(x => new string('=', x.Title?.Length ?? 0)).ToArray());
        }

        foreach (var item in items)
        {
            table.AddRow(columns.Select(x => x.GetValue(item)).ToArray());
        }

        ansiConsole.Write(table);
    }

    public static IAnsiConsole WriteLines(this IAnsiConsole ansiConsole, IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            ansiConsole.WriteLine(line);
        }

        return ansiConsole;
    }

    public static IAnsiConsole MarkupLines(this IAnsiConsole ansiConsole, IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            ansiConsole.MarkupLine(line);
        }

        return ansiConsole;
    }
}
