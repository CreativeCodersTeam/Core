using CreativeCoders.Core.Logging;
using CreativeCoders.Logging.Nlog;
using NLog.Config;
using NLog.Targets;
using Xunit;
using LogLevel = NLog.LogLevel;

namespace CreativeCoders.Core.UnitTests.Logging
{
    [Collection("Logging")]
    public class NlogLoggingTests : LoggingTestBase
    {
        [Fact]
        public void NLogLoggerTest()
        {
            Nlog.Init();
            var logger = LogManager.GetLogger(typeof(LoggingTests));
            var loggerScope = LogManager.GetLogger("TestScope");

            Assert.True(logger.IsDebugEnabled);
            Assert.True(logger.IsErrorEnabled);
            Assert.True(logger.IsFatalEnabled);
            Assert.True(logger.IsInfoEnabled);
            Assert.True(logger.IsTraceEnabled);
            Assert.True(logger.IsWarnEnabled);

            CallLogger(logger);
            CallLogger(loggerScope);
        }

        [Fact]
        public void NLogLoggerTestWithConfig()
        {
            var config = new LoggingConfiguration();
            Nlog.Init(config);

            var logger = LogManager.GetLogger(typeof(LoggingTests));
            var loggerScope = LogManager.GetLogger("TestScope");

            CallLogger(logger);
            CallLogger(loggerScope);
        }

        [Fact]
        public void NLogLoggerTestWithConfigAllLogLevelsEnabled()
        {
            var config = new LoggingConfiguration();
            config.AddTarget("null", new NullTarget());
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, "null");
            Nlog.Init(config);

            var logger = LogManager.GetLogger(typeof(LoggingTests));
            var loggerScope = LogManager.GetLogger("TestScope");

            CallLogger(logger);
            CallLogger(loggerScope);
        }
    }
}