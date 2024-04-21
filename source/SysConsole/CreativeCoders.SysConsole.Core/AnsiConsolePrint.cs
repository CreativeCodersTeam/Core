using System;
using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Core;
using Spectre.Console;
using Spectre.Console.Advanced;
using Spectre.Console.Rendering;

namespace CreativeCoders.SysConsole.Core;

[ExcludeFromCodeCoverage]
public class AnsiConsolePrint(IAnsiConsole ansiConsole) : IAnsiConsolePrint
{
    private readonly IAnsiConsole _ansiConsole = Ensure.NotNull(ansiConsole);

    public IAnsiConsolePrint WriteLine()
    {
        _ansiConsole.WriteLine();

        return this;
    }

    public IAnsiConsolePrint WriteLine(string text)
    {
        _ansiConsole.WriteLine();

        return this;
    }

    public IAnsiConsolePrint WriteLine(string text, Style? style)
    {
        _ansiConsole.WriteLine(text, style);

        return this;
    }

    public IAnsiConsolePrint Write(IRenderable renderable)
    {
        _ansiConsole.Write(renderable);

        return this;
    }

    public IAnsiConsolePrint Write(string text)
    {
        _ansiConsole.Write(text);

        return this;
    }

    public IAnsiConsolePrint Write(string text, Style? style)
    {
        _ansiConsole.Write(text, style);

        return this;
    }

    public IAnsiConsolePrint Markup(string value)
    {
        _ansiConsole.Markup(value);

        return this;
    }

    public IAnsiConsolePrint Markup(string format, params object[] args)
    {
        _ansiConsole.Markup(format, args);

        return this;
    }

    public IAnsiConsolePrint Markup(IFormatProvider provider, string format, params object[] args)
    {
        _ansiConsole.Markup(provider, format, args);

        return this;
    }

    public IAnsiConsolePrint MarkupLine(string value)
    {
        _ansiConsole.MarkupLine(value);

        return this;
    }

    public IAnsiConsolePrint MarkupLine(string format, params object[] args)
    {
        _ansiConsole.MarkupLine(format, args);

        return this;
    }

    public IAnsiConsolePrint MarkupLine(IFormatProvider provider, string format, params object[] args)
    {
        _ansiConsole.MarkupLine(provider, format, args);

        return this;
    }

    public IAnsiConsolePrint MarkupInterpolated(FormattableString value)
    {
        _ansiConsole.MarkupInterpolated(value);

        return this;
    }

    public IAnsiConsolePrint MarkupInterpolated(IFormatProvider provider, FormattableString value)
    {
        _ansiConsole.MarkupInterpolated(provider, value);

        return this;
    }

    public IAnsiConsolePrint MarkupLineInterpolated(FormattableString value)
    {
        _ansiConsole.MarkupLineInterpolated(value);

        return this;
    }

    public IAnsiConsolePrint MarkupLineInterpolated(IFormatProvider provider, FormattableString value)
    {
        _ansiConsole.MarkupLineInterpolated(provider, value);

        return this;
    }

    public IAnsiConsolePrint WriteException(Exception exception,
        ExceptionFormats format = ExceptionFormats.Default)
    {
        _ansiConsole.WriteException(exception, format);

        return this;
    }

    public IAnsiConsolePrint WriteException(Exception exception, ExceptionSettings settings)
    {
        _ansiConsole.WriteException(exception, settings);

        return this;
    }

    public IAnsiConsolePrint WriteAnsi(string sequence)
    {
        _ansiConsole.WriteAnsi(sequence);

        return this;
    }
}
