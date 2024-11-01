using CreativeCoders.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.SysConsole.Core;

[PublicAPI]
public static class AnsiConsolePrintExtensions
{
    public static IAnsiConsolePrint Write<T>(this IAnsiConsolePrint ansiConsolePrint, T value,
        Color foregroundColor,
        Color? backgroundColor = null)
    {
        var style = new Style(foreground: foregroundColor, background: backgroundColor);

        ansiConsolePrint.Write(value.ToStringSafe(), style);

        return ansiConsolePrint;
    }

    public static IAnsiConsolePrint WriteLine<T>(this IAnsiConsolePrint ansiConsolePrint, T value,
        Color foregroundColor,
        Color? backgroundColor = null)
    {
        var style = new Style(foreground: foregroundColor, background: backgroundColor);

        ansiConsolePrint.WriteLine(value.ToStringSafe(), style);

        return ansiConsolePrint;
    }
}
