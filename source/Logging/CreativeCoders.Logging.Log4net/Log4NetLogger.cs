using System;
using CreativeCoders.Core;
using CreativeCoders.Core.Logging;
using log4net;
using log4net.Core;
using ILogger = CreativeCoders.Core.Logging.ILogger;

namespace CreativeCoders.Logging.Log4net;

internal class Log4NetLogger : ILogger
{
    private readonly ILog _log;

    public Log4NetLogger(ILog log)
    {
        Ensure.IsNotNull(log, "log");
        _log = log;
    }

    public bool IsTraceEnabled => _log.Logger.IsEnabledFor(Level.Trace);

    public bool IsDebugEnabled => _log.IsDebugEnabled;

    public bool IsInfoEnabled => _log.IsInfoEnabled;

    public bool IsWarnEnabled => _log.IsWarnEnabled;

    public bool IsErrorEnabled => _log.IsErrorEnabled;

    public bool IsFatalEnabled => _log.IsFatalEnabled;

    public void Log(LogLevel logLevel, string message)
    {
        var log4NetLogLevel = LogLevelToLog4NetLogLevel(logLevel);
        _log.Log(log4NetLogLevel, message);
    }

    public void Log(LogLevel logLevel, string message, Exception exception)
    {
        _log.Log(LogLevelToLog4NetLogLevel(logLevel), message);
    }

    public void LogFormat(LogLevel logLevel, string format, params object[] args)
    {
        var log4NetLogLevel = LogLevelToLog4NetLogLevel(logLevel);
        _log.LogFormat(log4NetLogLevel, format, args);
    }

    public void Log<T>(LogLevel logLevel, T value)
    {
        _log.Log(LogLevelToLog4NetLogLevel(logLevel), value);
    }

    public void Log<T>(LogLevel logLevel, Func<T> getMessage)
    {
        var log4NetLogLevel = LogLevelToLog4NetLogLevel(logLevel);
        if (_log.Logger.IsEnabledFor(log4NetLogLevel))
        {
            _log.Log(log4NetLogLevel, getMessage());
        }
    }

    public void Trace(string message)
    {
        _log.Trace(message);
    }

    public void Trace(string message, Exception exception)
    {
        _log.Trace(message, exception);
    }

    public void TraceFormat(string format, params object[] args)
    {
        _log.TraceFormat(format, args);
    }

    public void Trace<T>(T value)
    {
        _log.Trace(value);
    }

    public void Trace<T>(Func<T> getMessage)
    {
        if (IsTraceEnabled)
        {
            _log.Trace(getMessage());
        }
    }

    public void Debug(string message)
    {
        _log.Debug(message);
    }

    public void Debug(string message, Exception exception)
    {
        _log.Debug(message, exception);
    }

    public void DebugFormat(string format, params object[] args)
    {
        _log.DebugFormat(format, args);
    }

    public void Debug<T>(T value)
    {
        _log.Debug(value);
    }

    public void Debug<T>(Func<T> getMessage)
    {
        if (IsDebugEnabled)
        {
            _log.Debug(getMessage());
        }
    }

    public void Info(string message)
    {
        _log.Info(message);
    }

    public void Info(string message, Exception exception)
    {
        _log.Info(message, exception);
    }

    public void InfoFormat(string format, params object[] args)
    {
        _log.InfoFormat(format, args);
    }

    public void Info<T>(T value)
    {
        _log.Info(value);
    }

    public void Info<T>(Func<T> getMessage)
    {
        if (IsInfoEnabled)
        {
            _log.Info(getMessage());
        }
    }

    public void Warn(string message)
    {
        _log.Warn(message);
    }

    public void Warn(string message, Exception exception)
    {
        _log.Warn(message, exception);
    }

    public void WarnFormat(string format, params object[] args)
    {
        _log.WarnFormat(format, args);
    }

    public void Warn<T>(T value)
    {
        _log.Warn(value);
    }

    public void Warn<T>(Func<T> getMessage)
    {
        if (IsWarnEnabled)
        {
            _log.Warn(getMessage());
        }
    }

    public void Error(string message)
    {
        _log.Error(message);
    }

    public void Error(string message, Exception exception)
    {
        _log.Error(message, exception);
    }

    public void ErrorFormat(string format, params object[] args)
    {
        _log.ErrorFormat(format, args);
    }

    public void Error<T>(T value)
    {
        _log.Error(value);
    }

    public void Error<T>(Func<T> getMessage)
    {
        if (IsErrorEnabled)
        {
            _log.Error(getMessage());
        }
    }

    public void Fatal(string message)
    {
        _log.Fatal(message);
    }

    public void Fatal(string message, Exception exception)
    {
        _log.Fatal(message, exception);
    }

    public void FatalFormat(string format, params object[] args)
    {
        _log.FatalFormat(format, args);
    }

    public void Fatal<T>(T value)
    {
        _log.Fatal(value);
    }

    public void Fatal<T>(Func<T> getMessage)
    {
        if (IsFatalEnabled)
        {
            _log.Fatal(getMessage());
        }
    }

    private static Level LogLevelToLog4NetLogLevel(LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Trace => Level.Trace,
            LogLevel.Debug => Level.Debug,
            LogLevel.Info => Level.Info,
            LogLevel.Warn => Level.Warn,
            LogLevel.Error => Level.Error,
            LogLevel.Fatal => Level.Fatal,
            _ => Level.Info
        };
    }
}
