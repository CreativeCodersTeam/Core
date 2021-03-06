﻿using System;
using CreativeCoders.Core;
using NLog;
using ILogger = CreativeCoders.Core.Logging.ILogger;
using LogLevel = CreativeCoders.Core.Logging.LogLevel;

namespace CreativeCoders.Logging.Nlog
{
    internal class NlogLogger : ILogger
    {
        private readonly Logger _logger;

        public NlogLogger(Logger logger)
        {
            Ensure.IsNotNull(logger, nameof(logger));
            _logger = logger;
        }

        public bool IsTraceEnabled => _logger.IsTraceEnabled;

        public bool IsDebugEnabled => _logger.IsDebugEnabled;

        public bool IsInfoEnabled => _logger.IsInfoEnabled;

        public bool IsWarnEnabled => _logger.IsWarnEnabled;

        public bool IsErrorEnabled => _logger.IsErrorEnabled;

        public bool IsFatalEnabled => _logger.IsFatalEnabled;

        public void Log(LogLevel logLevel, string message)
        {
            _logger.Log(LogLevelToNlogLogLevel(logLevel), message);
        }

        public void Log(LogLevel logLevel, string message, Exception exception)
        {
            _logger.Log(LogLevelToNlogLogLevel(logLevel), exception, message);
        }

        public void LogFormat(LogLevel logLevel, string format, params object[] args)
        {
            _logger.Log(LogLevelToNlogLogLevel(logLevel), format, args);
        }

        public void Log<T>(LogLevel logLevel, T value)
        {
            _logger.Log(LogLevelToNlogLogLevel(logLevel), value);
        }

        public void Log<T>(LogLevel logLevel, Func<T> getMessage)
        {
            _logger.Log(LogLevelToNlogLogLevel(logLevel), getMessage());
        }

        public void Trace(string message)
        {
            _logger.Trace(message);
        }

        public void Trace(string message, Exception exception)
        {
            _logger.Trace(exception, message);
        }

        public void TraceFormat(string format, params object[] args)
        {
            _logger.Trace(format, args);
        }

        public void Trace<T>(T value)
        {
            _logger.Trace(value);
        }

        public void Trace<T>(Func<T> getMessage)
        {
            _logger.Trace(getMessage());
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Debug(string message, Exception exception)
        {
            _logger.Debug(exception, message);
        }

        public void DebugFormat(string format, params object[] args)
        {
            _logger.Debug(format, args);
        }

        public void Debug<T>(T value)
        {
            _logger.Debug(value);
        }

        public void Debug<T>(Func<T> getMessage)
        {
            _logger.Debug(getMessage());
        }

        public void Info(string message)
        {
            _logger.Info(message);
        }

        public void Info(string message, Exception exception)
        {
            _logger.Info(exception, message);
        }

        public void InfoFormat(string format, params object[] args)
        {
            _logger.Info(format, args);
        }

        public void Info<T>(T value)
        {
            _logger.Info(value);
        }

        public void Info<T>(Func<T> getMessage)
        {
            _logger.Info(getMessage());
        }

        public void Warn(string message)
        {
            _logger.Warn(message);
        }

        public void Warn(string message, Exception exception)
        {
            _logger.Warn(exception, message);
        }

        public void WarnFormat(string format, params object[] args)
        {
            _logger.Warn(format, args);
        }

        public void Warn<T>(T value)
        {
            _logger.Warn(value);
        }

        public void Warn<T>(Func<T> getMessage)
        {
            _logger.Warn(getMessage());
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            _logger.Error(exception, message);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            _logger.Error(format, args);
        }

        public void Error<T>(T value)
        {
            _logger.Error(value);
        }

        public void Error<T>(Func<T> getMessage)
        {
            _logger.Error(getMessage());
        }

        public void Fatal(string message)
        {
            _logger.Fatal(message);
        }

        public void Fatal(string message, Exception exception)
        {
            _logger.Fatal(exception, message);
        }

        public void FatalFormat(string format, params object[] args)
        {
            _logger.Fatal(format, args);
        }

        public void Fatal<T>(T value)
        {
            _logger.Fatal(value);
        }

        public void Fatal<T>(Func<T> getMessage)
        {
            _logger.Fatal(getMessage());
        }

        private static NLog.LogLevel LogLevelToNlogLogLevel(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Trace => NLog.LogLevel.Trace,
                LogLevel.Debug => NLog.LogLevel.Debug,
                LogLevel.Info => NLog.LogLevel.Info,
                LogLevel.Warn => NLog.LogLevel.Warn,
                LogLevel.Error => NLog.LogLevel.Error,
                LogLevel.Fatal => NLog.LogLevel.Fatal,
                _ => NLog.LogLevel.Off
            };
        }
    }
}