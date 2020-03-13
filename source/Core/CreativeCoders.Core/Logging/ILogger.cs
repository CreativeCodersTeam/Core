using System;

namespace CreativeCoders.Core.Logging
{
    public interface ILogger
    {
        bool IsTraceEnabled { get; }

        bool IsDebugEnabled { get; }

        bool IsInfoEnabled { get; }

        bool IsWarnEnabled { get; }

        bool IsErrorEnabled { get; }

        bool IsFatalEnabled { get; }

        void Log(LogLevel logLevel, string message);

        void Log(LogLevel logLevel, string message, Exception exception);

        void LogFormat(LogLevel logLevel, string format, params object[] args);

        void Log<T>(LogLevel logLevel, T value);

        void Log<T>(LogLevel logLevel, Func<T> getMessage);

        void Trace(string message);

        void Trace(string message, Exception exception);

        void TraceFormat(string format, params object[] args);

        void Trace<T>(T value);

        void Trace<T>(Func<T> getMessage);

        void Debug(string message);

        void Debug(string message, Exception exception);

        void DebugFormat(string format, params object[] args);

        void Debug<T>(T value);

        void Debug<T>(Func<T> getMessage);

        void Info(string message);

        void Info(string message, Exception exception);

        void InfoFormat(string format, params object[] args);

        void Info<T>(T value);

        void Info<T>(Func<T> getMessage);

        void Warn(string message);

        void Warn(string message, Exception exception);

        void WarnFormat(string format, params object[] args);

        void Warn<T>(T value);

        void Warn<T>(Func<T> getMessage);

        void Error(string message);

        void Error(string message, Exception exception);

        void ErrorFormat(string format, params object[] args);

        void Error<T>(T value);

        void Error<T>(Func<T> getMessage);

        void Fatal(string message);

        void Fatal(string message, Exception exception);

        void FatalFormat(string format, params object[] args);

        void Fatal<T>(T value);

        void Fatal<T>(Func<T> getMessage);
    }
}