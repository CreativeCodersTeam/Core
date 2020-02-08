using CreativeCoders.Core.Logging;

namespace CreativeCoders.Logging.DebuggerOutput
{
    public static class DebuggerOutputLog
    {
        public static void Init()
        {
            LogManager.SetLoggerFactory(new DebuggerOutputLoggerFactory());
        }

        public static bool IsTraceEnabled { get; set; } = true;

        public static bool IsDebugEnabled { get; set; } = true;

        public static bool IsInfoEnabled { get; set; } = true;

        public static bool IsWarnEnabled { get; set; } = true;

        public static bool IsErrorEnabled { get; set; } = true;

        public static bool IsFatalEnabled { get; set; } = true;
    }
}
