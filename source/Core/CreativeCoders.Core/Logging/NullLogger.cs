using System;

namespace CreativeCoders.Core.Logging
{
    internal class NullLogger : ILogger
    {
        public bool IsTraceEnabled => false;

        public bool IsDebugEnabled => false;

        public bool IsInfoEnabled => false;

        public bool IsWarnEnabled => false;

        public bool IsErrorEnabled => false;

        public bool IsFatalEnabled => false;

        public void Log(LogLevel logLevel, string message) {}

        public void Log(LogLevel logLevel, string message, Exception exception) {}

        public void LogFormat(LogLevel logLevel, string format, params object[] args) {}

        public void Log<T>(LogLevel logLevel, T value) {}

        public void Log<T>(LogLevel logLevel, Func<T> messageFunc) {}

        public void Trace(string message) {}

        public void Trace(string message, Exception exception) {}

        public void TraceFormat(string format, params object[] args) {}

        public void Trace<T>(T value) {}

        public void Trace<T>(Func<T> messageFunc) {}

        public void Debug(string message) {}

        public void Debug(string message, Exception exception) {}

        public void DebugFormat(string format, params object[] args) {}

        public void Debug<T>(T value) {}

        public void Debug<T>(Func<T> messageFunc) {}

        public void Info(string message) {}

        public void Info(string message, Exception exception) {}

        public void InfoFormat(string format, params object[] args) {}

        public void Info<T>(T value) {}

        public void Info<T>(Func<T> messageFunc) {}

        public void Warn(string message) {}

        public void Warn(string message, Exception exception) {}

        public void WarnFormat(string format, params object[] args) {}

        public void Warn<T>(T value) {}

        public void Warn<T>(Func<T> messageFunc) {}

        public void Error(string message) {}

        public void Error(string message, Exception exception) {}

        public void ErrorFormat(string format, params object[] args) {}

        public void Error<T>(T value) {}

        public void Error<T>(Func<T> messageFunc) {}

        public void Fatal(string message) {}

        public void Fatal(string message, Exception exception) {}

        public void FatalFormat(string format, params object[] args) {}

        public void Fatal<T>(T value) {}

        public void Fatal<T>(Func<T> messageFunc) {}
    }
}