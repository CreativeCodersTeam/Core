using CreativeCoders.Core.Logging;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Logging;

[Collection("Logging")]
public class NullLoggingTests : LoggingTestBase
{
    [Fact]
    public void NullLoggerTest()
    {
        LogManager.SetLoggerFactory(null);
        var logger = LogManager.GetLogger(typeof(LoggingTests));

        Assert.False(logger.IsDebugEnabled);
        Assert.False(logger.IsErrorEnabled);
        Assert.False(logger.IsFatalEnabled);
        Assert.False(logger.IsInfoEnabled);
        Assert.False(logger.IsTraceEnabled);
        Assert.False(logger.IsWarnEnabled);

        CallLogger(logger);
    }
}
