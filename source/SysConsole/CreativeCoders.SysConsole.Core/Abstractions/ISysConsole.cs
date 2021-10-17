using System;
using JetBrains.Annotations;

namespace CreativeCoders.SysConsole.Core.Abstractions
{
    [PublicAPI]
    public interface ISysConsole
    {
        ISysConsole WriteLine();

        ISysConsole WriteLine<T>(T? data);

        ISysConsole Write<T>(T? data);

        ISysConsole WriteError<T>(T? data);

        ISysConsole WriteLineError<T>(T? data);

        ConsoleKeyInfo ReadKey();

        ConsoleKeyInfo ReadKey(bool intercept);

        string? ReadLine();

        ISysConsole Clear();

        ISysConsole ResetColor();

        ConsoleColor ForegroundColor { get; set; }

        ConsoleColor BackgroundColor { get; set; }

        ConsoleColor ErrorForegroundColor { get; set; }

        ConsoleColor ErrorBackgroundColor { get; set; }

        string Title { get; set; }

        int BufferHeight { get; set; }

        int BufferWidth { get; set; }

        int CursorLeft { get; set; }

        int CursorTop { get; set; }

        int WindowHeight { get; set; }

        int WindowWidth { get; set; }

        int WindowLeft { get; set; }

        int WindowTop { get; set; }
    }
}