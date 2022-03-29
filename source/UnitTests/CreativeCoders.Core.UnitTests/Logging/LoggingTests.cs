using CreativeCoders.Core.Logging;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Logging;

[Collection("Logging")]
public class LoggingTests : LoggingTestBase
{
    [Fact]
    public void LogManagerTest()
    {
        LogManager.SetLoggerFactory(null);

        var loggerForScope = LogManager.GetLogger("TestScope");
        var loggerForType = LogManager.GetLogger(typeof(LoggingTests));

        Assert.NotNull(loggerForScope);
        Assert.NotNull(loggerForType);

        Assert.True(loggerForScope.GetType().Name == "NullLogger");

        CallLogger(loggerForType);

        CallLogger(loggerForScope);
    }
}