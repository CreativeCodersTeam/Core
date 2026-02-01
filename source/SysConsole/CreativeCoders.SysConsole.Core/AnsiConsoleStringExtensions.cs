using System.Text;
using Spectre.Console;

namespace CreativeCoders.SysConsole.Core;

public static class AnsiConsoleStringExtensions
{
    public static string ToErrorMarkup(this string text)
    {
        return $"[red]{text}[/]";
    }

    public static string ToSuccessMarkup(this string text)
    {
        return $"[green]{text}[/]";
    }

    public static string ToWarningMarkup(this string text)
    {
        return $"[yellow]{text}[/]";
    }

    public static string ToInfoMarkup(this string text)
    {
        return $"[blue]{text}[/]";
    }

    public static string ToWhiteMarkup(this string text)
    {
        return $"[white]{text}[/]";
    }

    public static string ToBoldMarkup(this string text)
    {
        return $"[bold]{text}[/]";
    }

    public static string ToItalicMarkup(this string text)
    {
        return $"[italic]{text}[/]";
    }

    public static string ToUnderlineMarkup(this string text)
    {
        return $"[underline]{text}[/]";
    }

    public static string ToStrikethroughMarkup(this string text)
    {
        return $"[strikethrough]{text}[/]";
    }

    public static string ToLinkMarkup(this string text, string url = "")
    {
        return string.IsNullOrWhiteSpace(url)
            ? $"[link]{text}[/]"
            : $"[link={url}]{text}[/]";
    }

    public static string ToEscapedMarkup(this string text)
    {
        return Markup.Escape(text);
    }
}
