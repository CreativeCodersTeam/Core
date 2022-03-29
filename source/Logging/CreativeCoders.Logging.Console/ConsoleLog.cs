using CreativeCoders.Core.Logging;

namespace CreativeCoders.Logging.Console;

public static class ConsoleLog
{
    public static void Init()
    {
        LogManager.SetLoggerFactory(new ConsoleLoggerFactory());
    }

    public static bool IsTraceEnabled { get; set; } = true;

    public static bool IsDebugEnabled { get; set; } = true;

    public static bool IsInfoEnabled { get; set; } = true;

    public static bool IsWarnEnabled { get; set; } = true;

    public static bool IsErrorEnabled { get; set; } = true;

    public static bool IsFatalEnabled { get; set; } = true;
}