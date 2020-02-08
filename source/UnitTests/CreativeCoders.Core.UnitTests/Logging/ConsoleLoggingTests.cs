using CreativeCoders.Core.Logging;
using CreativeCoders.Logging.Console;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Logging
{
    [Collection("Logging")]
    public class ConsoleLoggingTests : LoggingTestBase
    {
        [Fact]
        public void ConsoleLoggerTest()
        {
            ConsoleLog.Init();
            var logger = LogManager.GetLogger(typeof(LoggingTests));
            var _ = LogManager.GetLogger("TestScope");

            Assert.True(logger.IsDebugEnabled);
            Assert.True(logger.IsErrorEnabled);
            Assert.True(logger.IsFatalEnabled);
            Assert.True(logger.IsInfoEnabled);
            Assert.True(logger.IsTraceEnabled);
            Assert.True(logger.IsWarnEnabled);

            CallLogger(logger);

            ConsoleLog.IsDebugEnabled = false;
            ConsoleLog.IsErrorEnabled = false;
            ConsoleLog.IsFatalEnabled = false;
            ConsoleLog.IsInfoEnabled = false;
            ConsoleLog.IsTraceEnabled = false;
            ConsoleLog.IsWarnEnabled = false;

            CallLogger(logger);
        }
    }
}