using System;
using CreativeCoders.Core.Logging;

namespace CreativeCoders.Logging.Delegate
{
    internal class DelegateLogger : ISimpleLoggerImpl
    {
        private readonly string _scope;

        private readonly Action<LogLevel, string, string> _logAction;

        public DelegateLogger(string scope, Action<LogLevel, string, string> logAction)
        {
            _scope = scope;
            _logAction = logAction;
        }

        public bool IsTraceEnabled => DelegateLog.IsTraceEnabled;

        public bool IsDebugEnabled => DelegateLog.IsDebugEnabled;

        public bool IsInfoEnabled => DelegateLog.IsInfoEnabled;

        public bool IsWarnEnabled => DelegateLog.IsWarnEnabled;

        public bool IsErrorEnabled => DelegateLog.IsErrorEnabled;

        public bool IsFatalEnabled => DelegateLog.IsFatalEnabled;

        public void Log(LogLevel logLevel, string text)
        {
            if (!IsLogLevelEnabled(logLevel))
            {
                return;
            }
            _logAction(logLevel, _scope, text);
        }

        public void Log(LogLevel logLevel, string text, Exception exception)
        {
            if (!IsLogLevelEnabled(logLevel))
            {
                return;
            }
            text = text + Environment.NewLine + exception;

            _logAction(logLevel, _scope, text);            
        }

        public void LogFormat(LogLevel logLevel, string formatText, params object[] args)
        {
            if (!IsLogLevelEnabled(logLevel))
            {
                return;
            }
            _logAction(logLevel, _scope, string.Format(formatText, args));
        }

        public void Log<T>(LogLevel logLevel, T value)
        {
            if (!IsLogLevelEnabled(logLevel))
            {
                return;
            }
            _logAction(logLevel, _scope, value.ToString());
        }

        public void Log<T>(LogLevel logLevel, Func<T> getMessage)
        {
            if (!IsLogLevelEnabled(logLevel))
            {
                return;
            }
            _logAction(logLevel, _scope, getMessage().ToString());
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
                    return false;
            }
        }
    }
}
