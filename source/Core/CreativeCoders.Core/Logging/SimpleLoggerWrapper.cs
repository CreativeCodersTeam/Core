using System;

namespace CreativeCoders.Core.Logging
{
    public class SimpleLoggerWrapper : ILogger
    {
        private readonly ISimpleLoggerImpl _loggerImpl;

        public SimpleLoggerWrapper(ISimpleLoggerImpl loggerImpl)
        {
            Ensure.IsNotNull(loggerImpl, "loggerImpl");
            _loggerImpl = loggerImpl;
        }

        public bool IsTraceEnabled => _loggerImpl.IsTraceEnabled;

        public bool IsDebugEnabled => _loggerImpl.IsDebugEnabled;

        public bool IsInfoEnabled => _loggerImpl.IsInfoEnabled;

        public bool IsWarnEnabled => _loggerImpl.IsWarnEnabled;

        public bool IsErrorEnabled => _loggerImpl.IsErrorEnabled;

        public bool IsFatalEnabled => _loggerImpl.IsFatalEnabled;

        public void Log(LogLevel logLevel, string message)
        {
            _loggerImpl.Log(logLevel, message);
        }

        public void Log(LogLevel logLevel, string message, Exception exception)
        {
            _loggerImpl.Log(logLevel, message, exception);
        }

        public void LogFormat(LogLevel logLevel, string format, params object[] args)
        {
            _loggerImpl.LogFormat(logLevel, format, args);
        }

        public void Log<T>(LogLevel logLevel, T value)
        {
            _loggerImpl.Log(logLevel, value);
        }

        public void Log<T>(LogLevel logLevel, Func<T> getMessage)
        {
            _loggerImpl.Log(logLevel, getMessage);
        }

        public void Trace(string message)
        {
            Log(LogLevel.Trace, message);
        }

        public void Trace(string message, Exception exception)
        {
            Log(LogLevel.Trace, message, exception);
        }

        public void TraceFormat(string format, params object[] args)
        {
            LogFormat(LogLevel.Trace, format, args);
        }

        public void Trace<T>(T value)
        {
            Log(LogLevel.Trace, value);
        }

        public void Trace<T>(Func<T> getMessage)
        {
            Trace(getMessage());
        }

        public void Debug(string message)
        {
            Log(LogLevel.Debug, message);
        }

        public void Debug(string message, Exception exception)
        {
            Log(LogLevel.Debug, message, exception);
        }

        public void DebugFormat(string format, params object[] args)
        {
            LogFormat(LogLevel.Debug, format, args);
        }

        public void Debug<T>(T value)
        {
            Log(LogLevel.Debug, value);
        }

        public void Debug<T>(Func<T> getMessage)
        {
            Debug(getMessage());
        }

        public void Info(string message)
        {
            Log(LogLevel.Info, message);
        }

        public void Info(string message, Exception exception)
        {
            Log(LogLevel.Info, message, exception);
        }

        public void InfoFormat(string format, params object[] args)
        {
            LogFormat(LogLevel.Info, format, args);
        }

        public void Info<T>(T value)
        {
            Log(LogLevel.Info, value);
        }

        public void Info<T>(Func<T> getMessage)
        {
            Info(getMessage());
        }

        public void Warn(string message)
        {
            Log(LogLevel.Warn, message);
        }

        public void Warn(string message, Exception exception)
        {
            Log(LogLevel.Warn, message, exception);
        }

        public void WarnFormat(string format, params object[] args)
        {
            LogFormat(LogLevel.Warn, format, args);
        }

        public void Warn<T>(T value)
        {
            Log(LogLevel.Warn, value);
        }

        public void Warn<T>(Func<T> getMessage)
        {
            Warn(getMessage());
        }

        public void Error(string message)
        {
            Log(LogLevel.Error, message);
        }

        public void Error(string message, Exception exception)
        {
            Log(LogLevel.Error, message, exception);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            LogFormat(LogLevel.Error, format, args);
        }

        public void Error<T>(T value)
        {
            Log(LogLevel.Error, value);
        }

        public void Error<T>(Func<T> getMessage)
        {
            if (IsErrorEnabled)
            {
                Error(getMessage());
            }
        }

        public void Fatal(string message)
        {
            Log(LogLevel.Fatal, message);
        }

        public void Fatal(string message, Exception exception)
        {
            Log(LogLevel.Fatal, message, exception);
        }

        public void FatalFormat(string format, params object[] args)
        {
            LogFormat(LogLevel.Fatal, format, args);
        }

        public void Fatal<T>(T value)
        {
            Log(LogLevel.Fatal, value);
        }

        public void Fatal<T>(Func<T> getMessage)
        {
            if (IsFatalEnabled)
            {
                Fatal(getMessage());
            }
        }
    }
}