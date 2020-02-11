using System;
using System.Diagnostics;
using CreativeCoders.Core.Logging;

namespace CreativeCoders.Logging.DebuggerOutput
{
    public class DebuggerOutputLogger : ISimpleLoggerImpl
    {
        private readonly string _scope;

        public DebuggerOutputLogger(string scope)
        {
            _scope = scope;
        }

        public bool IsTraceEnabled => DebuggerOutputLog.IsTraceEnabled;

        public bool IsDebugEnabled => DebuggerOutputLog.IsDebugEnabled;

        public bool IsInfoEnabled => DebuggerOutputLog.IsInfoEnabled;

        public bool IsWarnEnabled => DebuggerOutputLog.IsWarnEnabled;

        public bool IsErrorEnabled => DebuggerOutputLog.IsErrorEnabled;

        public bool IsFatalEnabled => DebuggerOutputLog.IsFatalEnabled;

        public void Log(LogLevel logLevel, string text)
        {
            if (!IsLogLevelEnabled(logLevel))
            {
                return;
            }
            WriteText(logLevel, text);
        }

        public void Log(LogLevel logLevel, string text, Exception exception)
        {
            if (!IsLogLevelEnabled(logLevel))
            {
                return;
            }
            WriteText(logLevel, text);
            Debug.WriteLine(exception);
        }

        public void LogFormat(LogLevel logLevel, string formatText, params object[] args)
        {
            if (!IsLogLevelEnabled(logLevel))
            {
                return;
            }
            WriteText(logLevel, string.Format(formatText, args));
        }

        public void Log<T>(LogLevel logLevel, T value)
        {
            if (!IsLogLevelEnabled(logLevel))
            {
                return;
            }
            WriteText(logLevel, value.ToString());
        }

        public void Log<T>(LogLevel logLevel, Func<T> getMessage)
        {
            if (!IsLogLevelEnabled(logLevel))
            {
                return;
            }
            WriteText(logLevel, getMessage().ToString());
        }

        private void WriteText(LogLevel logLevel, string text)
        {
            Debug.WriteLine($"{DateTime.Now:dd.MM.yyyy HH:mm:ss} {_scope} {logLevel} {text}");
        }

        private bool IsLogLevelEnabled(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return IsTraceEnabled;
                case LogLevel.Debug:
                    return IsDebugEnabled;
                case LogLevel.Info:
                    return IsInfoEnabled;
                case LogLevel.Warn:
                    return IsWarnEnabled;
                case LogLevel.Error:
                    return IsErrorEnabled;
                case LogLevel.Fatal:
                    return IsFatalEnabled;
                default:
                    return true;
            }
        }
    }
}
