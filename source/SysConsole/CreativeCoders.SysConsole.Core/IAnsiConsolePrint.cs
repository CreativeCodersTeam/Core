using System;
using JetBrains.Annotations;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace CreativeCoders.SysConsole.Core;

[PublicAPI]
public interface IAnsiConsolePrint
{
    IAnsiConsolePrint WriteLine();

    IAnsiConsolePrint WriteLine(string text);

    IAnsiConsolePrint WriteLine(string text, Style? style);

    IAnsiConsolePrint Write(IRenderable renderable);

    IAnsiConsolePrint Write(string text);

    IAnsiConsolePrint Write(string text, Style? style);

    IAnsiConsolePrint Markup(string value);

    IAnsiConsolePrint Markup(string format, params object[] args);

    IAnsiConsolePrint Markup(IFormatProvider provider, string format, params object[] args);

    IAnsiConsolePrint MarkupLine(string value);

    IAnsiConsolePrint MarkupLine(string format, params object[] args);

    IAnsiConsolePrint MarkupLine(IFormatProvider provider, string format, params object[] args);

    IAnsiConsolePrint MarkupInterpolated(FormattableString value);

    IAnsiConsolePrint MarkupInterpolated(IFormatProvider provider, FormattableString value);

    IAnsiConsolePrint MarkupLineInterpolated(FormattableString value);

    IAnsiConsolePrint MarkupLineInterpolated(IFormatProvider provider, FormattableString value);

    IAnsiConsolePrint WriteException(Exception exception, ExceptionFormats format = ExceptionFormats.Default);

    IAnsiConsolePrint WriteException(Exception exception, ExceptionSettings settings);

    IAnsiConsolePrint WriteAnsi(string sequence);
}
