using System;
using CreativeCoders.Core.Logging;

namespace CreativeCoders.Logging.Console
{
    internal class ConsoleLogger : ISimpleLoggerImpl
    {
        private readonly string _scope;

        public ConsoleLogger(string scope)
        {
            _scope = scope;
        }

        public bool IsTraceEnabled => ConsoleLog.IsTraceEnabled;

        public bool IsDebugEnabled => ConsoleLog.IsDebugEnabled;

        public bool IsInfoEnabled => ConsoleLog.IsInfoEnabled;

        public bool IsWarnEnabled => ConsoleLog.IsWarnEnabled;

        public bool IsErrorEnabled => ConsoleLog.IsErrorEnabled;

        public bool IsFatalEnabled => ConsoleLog.IsFatalEnabled;

        public void Log(LogLevel logLevel, string text)
        {
            var oldForeColor = System.Console.ForegroundColor;
            var oldBackColor = System.Console.BackgroundColor;
            System.Console.ForegroundColor = GetForegroundColorForLogLevel(logLevel);
            System.Console.BackgroundColor = GetBackgroundColorForLogLevel(logLevel);
            System.Console.WriteLine("{0} {1}: {2}", _scope, logLevel, text);
            System.Console.ForegroundColor = oldForeColor;
            System.Console.BackgroundColor = oldBackColor;
        }

        private static ConsoleColor GetForegroundColorForLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return ConsoleColor.DarkGray;
                case LogLevel.Debug:
                    return ConsoleColor.Cyan;
                case LogLevel.Warn:
                    return ConsoleColor.Yellow;
                case LogLevel.Error:
                    return ConsoleColor.Red;
                case LogLevel.Fatal:
                    return ConsoleColor.Red;
                case LogLevel.Info:
                    return ConsoleColor.White;
                default:
                    return ConsoleColor.White;
            }
        }

        private static ConsoleColor GetBackgroundColorForLogLevel(LogLevel logLevel)
        {
            return logLevel == LogLevel.Error
                ? ConsoleColor.Gray
                : ConsoleColor.Black;
        }

        public void Log(LogLevel logLevel, string text, Exception exception)
        {
            Log(logLevel, $"{text}\n{exception}");
        }

        public void LogFormat(LogLevel logLevel, string formatText, params object[] args)
        {
            Log(logLevel, string.Format(formatText, args));
        }

        public void Log<T>(LogLevel logLevel, T value)
        {
            var obj = value as object;
            Log(logLevel, obj?.ToString() ?? string.Empty);
        }

        public void Log<T>(LogLevel logLevel, Func<T> messageFunc)
        {
            Log(logLevel, messageFunc());
        }
    }
}