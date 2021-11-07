using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.SysEnvironment;
using CreativeCoders.SysConsole.Core.Abstractions;

namespace CreativeCoders.SysConsole.Core.Default
{
    [ExcludeFromCodeCoverage]
    internal class DefaultSysConsole : ISysConsole
    {
        public ISysConsole WriteLine()
        {
            Console.WriteLine();

            return this;
        }

        public ISysConsole WriteLine<T>(T? data)
        {
            Console.WriteLine(data);

            return this;
        }

        public ISysConsole Write<T>(T? data)
        {
            Console.Write(data);

            return this;
        }

        public ISysConsole WriteError<T>(T? data)
        {
            return WithErrorColors(shellConsole => shellConsole.Write(data));
        }

        public ISysConsole WriteLineError<T>(T? data)
        {
            return WithErrorColors(shellConsole => shellConsole.WriteLine(data));
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            return Console.ReadKey(intercept);
        }

        public string? ReadLine()
        {
            return Console.ReadLine();
        }

        public ISysConsole Clear()
        {
            Console.Clear();

            return this;
        }

        public ISysConsole ResetColor()
        {
            Console.ResetColor();

            return this;
        }

        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        public ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        public ConsoleColor ErrorForegroundColor { get; set; } = ConsoleColor.Gray;

        public ConsoleColor ErrorBackgroundColor { get; set; } = ConsoleColor.Red;

        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public string Title
        {
            get => Env.OSVersion.Platform == PlatformID.Win32NT ? Console.Title : string.Empty;
            set => Console.Title = value;
        }

        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public int BufferHeight
        {
            get => Console.BufferHeight;
            set => ExecOnlyOnWindows(() => Console.BufferHeight = value);
        }


        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public int BufferWidth
        {
            get => Console.BufferWidth;
            set => ExecOnlyOnWindows(() => Console.BufferWidth = value);
        }

        public int CursorLeft
        {
            get => Console.CursorLeft;
            set => Console.CursorLeft = value;
        }

        public int CursorTop
        {
            get => Console.CursorTop;
            set => Console.CursorTop = value;
        }

        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public int WindowHeight
        {
            get => Console.WindowHeight;
            set => ExecOnlyOnWindows(() => Console.WindowHeight = value);
        }

        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public int WindowWidth
        {
            get => Console.WindowWidth;
            set => ExecOnlyOnWindows(() => Console.WindowWidth = value);
        }

        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public int WindowLeft
        {
            get => Console.WindowLeft;
            set => ExecOnlyOnWindows(() => Console.WindowLeft = value);
        }

        [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        public int WindowTop
        {
            get => Console.WindowTop;
            set => ExecOnlyOnWindows(() => Console.WindowTop = value);
        }

        private ISysConsole WithErrorColors(Action<ISysConsole> actionWithErrorColors)
        {
            return WithColors(ErrorForegroundColor, ErrorBackgroundColor, actionWithErrorColors);
        }

        private ISysConsole WithColors(ConsoleColor foregroundColor, ConsoleColor backgroundColor, Action<ISysConsole> actionWithColors)
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;

            try
            {
                actionWithColors(this);
            }
            finally
            {
                ResetColor();
            }

            return this;
        }

        private static void ExecOnlyOnWindows(Action action)
        {
            if (Env.OSVersion.Platform != PlatformID.Win32NT)
            {
                return;
            }

            action();
        }
    }
}
