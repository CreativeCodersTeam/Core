using CreativeCoders.Core;
using JetBrains.Annotations;
using Spectre.Console;

namespace CreativeCoders.SysConsole.Core;

[PublicAPI]
public static class AnsiConsoleUtils
{
    public static string CreateMarkupText<T>(T value, Color foregroundColor, Color? backgroundColor = null)
    {
        var background = backgroundColor == null ? string.Empty : $" on {backgroundColor.Value.ToMarkup()}";

        return $"[{foregroundColor.ToMarkup()}{background}]{value.ToStringSafe()}[/]";
    }
}
