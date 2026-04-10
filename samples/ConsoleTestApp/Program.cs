using CreativeCoders.SysConsole.Core;
using Spectre.Console;

namespace ConsoleTestApp;

internal static class Program
{
    static void Main(string[] args)
    {
        var ansiConsole = AnsiConsole.Console;

        var style = new Style(decoration: Decoration.Bold | Decoration.Italic | Decoration.Strikethrough,
            foreground: Color.Blue, background: Color.Green);

        ansiConsole.PrintBlock()
            .WriteLine("Test")
            .Write("One")
            .Write("Two")
            .WriteLine("Three", Color.Green1)
            .Write("Four", Color.Red, Color.White)
            .Write("Five")
            .WriteLine("End")
            .WriteLine("2")
            .Write("Test1234", style);

        ansiConsole.Input.ReadKey(false);
    }
}
