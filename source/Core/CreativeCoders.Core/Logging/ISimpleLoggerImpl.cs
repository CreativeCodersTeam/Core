using System;

namespace CreativeCoders.Core.Logging;

public interface ISimpleLoggerImpl
{
    bool IsTraceEnabled { get; }

    bool IsDebugEnabled { get; }

    bool IsInfoEnabled { get; }

    bool IsWarnEnabled { get; }

    bool IsErrorEnabled { get; }

    bool IsFatalEnabled { get; }

    void Log(LogLevel logLevel, string text);

    void Log(LogLevel logLevel, string text, Exception exception);

    void LogFormat(LogLevel logLevel, string formatText, params object[] args);

    void Log<T>(LogLevel logLevel, T value);

    void Log<T>(LogLevel logLevel, Func<T> getMessage);
}