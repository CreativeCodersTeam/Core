using System;
using System.Collections.Generic;
using CreativeCoders.Core.Logging;
using CreativeCoders.Logging.Delegate;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Logging
{
    [Collection("Logging")]
    public class DelegateLoggingTest : LoggingTestBase
    {
        [Fact]
        public void DelegateInitTest()
        {
            Assert.Throws<ArgumentNullException>(() => DelegateLog.Init(null));
            DelegateLog.Init(DoLog);
        }

        [Fact]
        public void DelegateLoggerTest()
        {
            DelegateLog.Init(DoLog);
            var logger = LogManager.GetLogger<DelegateLoggingTest>();
            Assert.True(logger.IsDebugEnabled);
            Assert.True(logger.IsErrorEnabled);
            Assert.True(logger.IsFatalEnabled);
            Assert.True(logger.IsInfoEnabled);
            Assert.True(logger.IsTraceEnabled);
            Assert.True(logger.IsWarnEnabled);

            logger.Debug("Debug");
            logger.Info("Info");
            logger.Error("Error");
            logger.Fatal("Fatal");
            logger.Trace("Trace");
            logger.Warn("Warn");

            Assert.True(_logEntries[LogLevel.Debug] == "Debug");
            Assert.True(_logEntries[LogLevel.Info] == "Info");
            Assert.True(_logEntries[LogLevel.Error] == "Error");
            Assert.True(_logEntries[LogLevel.Fatal] == "Fatal");
            Assert.True(_logEntries[LogLevel.Trace] == "Trace");
            Assert.True(_logEntries[LogLevel.Warn] == "Warn");

            _logEntries.Clear();

            CallLogger(logger);

            Assert.False(_logEntries.Count == 0);

            DelegateLog.IsDebugEnabled = false;
            DelegateLog.IsErrorEnabled = false;
            DelegateLog.IsFatalEnabled = false;
            DelegateLog.IsInfoEnabled = false;
            DelegateLog.IsTraceEnabled = false;
            DelegateLog.IsWarnEnabled = false;

            _logEntries.Clear();

            CallLogger(logger);

            Assert.True(_logEntries.Count == 0);

            var actualLogLevel = LogLevel.Info;
            DelegateLog.IsDebugEnabled = true;
            DelegateLog.Init((logLevel, _, _) => actualLogLevel = logLevel);
            logger = LogManager.GetLogger<DelegateLoggingTest>();
            logger.Log(LogLevel.Debug, "Test");

            Assert.Equal(LogLevel.Debug, actualLogLevel);
        }        

        private readonly IDictionary<LogLevel, string> _logEntries = new Dictionary<LogLevel, string>();

        private void DoLog(LogLevel logLevel, string scope, string message)
        {
            _logEntries[logLevel] = message;
        }
    }
}
