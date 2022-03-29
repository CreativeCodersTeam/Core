using CreativeCoders.Core.Logging;
using CreativeCoders.Logging.DebuggerOutput;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Logging;

[Collection("Logging")]
public class DebuggerOutputLoggingTests : LoggingTestBase
{
    [Fact]
    public void DebuggerOutputLogTest()
    {
        DebuggerOutputLog.Init();

        var logger = LogManager.GetLogger<DebuggerOutputLoggingTests>();
        var loggerFroScope = LogManager.GetLogger("TestScope");

        CallLogger(logger);
        CallLogger(loggerFroScope);

        DebuggerOutputLog.IsDebugEnabled = false;
        DebuggerOutputLog.IsErrorEnabled = false;
        DebuggerOutputLog.IsFatalEnabled = false;
        DebuggerOutputLog.IsInfoEnabled = false;
        DebuggerOutputLog.IsTraceEnabled = false;
        DebuggerOutputLog.IsWarnEnabled = false;

        CallLogger(logger);
        CallLogger(loggerFroScope);
    }
}