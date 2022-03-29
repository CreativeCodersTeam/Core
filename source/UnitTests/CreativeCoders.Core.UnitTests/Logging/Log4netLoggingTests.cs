using System.Reflection;
using CreativeCoders.Core.Logging;
using CreativeCoders.Logging.Log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Logging;

[Collection("Logging")]
public class Log4NetLoggingTests : LoggingTestBase
{
    [Fact]
    public void Log4NetLoggerTest()
    {
        Log4Net.Init();
        var logger = LogManager.GetLogger(typeof(LoggingTests));
        var loggerScope = LogManager.GetLogger("TestScope");

        Assert.False(logger.IsDebugEnabled);
        Assert.False(logger.IsErrorEnabled);
        Assert.False(logger.IsFatalEnabled);
        Assert.False(logger.IsInfoEnabled);
        Assert.False(logger.IsTraceEnabled);
        Assert.False(logger.IsWarnEnabled);

        CallLogger(logger);
        CallLogger(loggerScope);

        SetupLog4Net();
        CallLogger(logger);
        CallLogger(loggerScope);


        logger.Log((LogLevel)(-1), "test");
    }

    private static void SetupLog4Net()
    {
        var hierarchy = (Hierarchy)log4net.LogManager.GetRepository(Assembly.GetAssembly(typeof(Log4NetLoggingTests)));

        var patternLayout = new PatternLayout
        {
            ConversionPattern = "%date [%thread] %-5level %logger - %message%newline"
        };
        patternLayout.ActivateOptions();

        //RollingFileAppender roller = new RollingFileAppender();
        //roller.AppendToFile = false;
        //roller.File = @"Logs\EventLog.txt";
        //roller.Layout = patternLayout;
        //roller.MaxSizeRollBackups = 5;
        //roller.MaximumFileSize = "1GB";
        //roller.RollingStyle = RollingFileAppender.RollingMode.Size;
        //roller.StaticLogFileName = true;
        //roller.ActivateOptions();
        //hierarchy.Root.AddAppender(roller);

        var memory = new MemoryAppender();
        memory.ActivateOptions();
        hierarchy.Root.AddAppender(memory);

        hierarchy.Root.Level = Level.All;
        hierarchy.Configured = true;
    }
}