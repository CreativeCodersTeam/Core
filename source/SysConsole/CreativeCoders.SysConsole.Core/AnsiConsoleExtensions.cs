using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Core;
using Spectre.Console;

namespace CreativeCoders.SysConsole.Core;

[ExcludeFromCodeCoverage]
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
}
