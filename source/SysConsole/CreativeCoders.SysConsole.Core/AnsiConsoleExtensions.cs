using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.SysConsole.Core;

[ExcludeFromCodeCoverage]
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

    public static IAnsiConsole Write<T>(this IAnsiConsole ansiConsole, T value, Color foregroundColor,
        Color? backgroundColor = null)
    {
        var style = new Style(foreground: foregroundColor, background: backgroundColor);

        ansiConsole.Write(value.ToStringSafe(), style);

        return ansiConsole;
    }

    public static IAnsiConsole WriteLine<T>(this IAnsiConsole ansiConsole, T value, Color foregroundColor,
        Color? backgroundColor = null)
    {
        var style = new Style(foreground: foregroundColor, background: backgroundColor);

        ansiConsole.WriteLine(value.ToStringSafe(), style);

        return ansiConsole;
    }
}
