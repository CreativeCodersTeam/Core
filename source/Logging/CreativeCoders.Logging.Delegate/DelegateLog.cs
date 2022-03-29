using System;
using CreativeCoders.Core;
using CreativeCoders.Core.Logging;
using JetBrains.Annotations;

namespace CreativeCoders.Logging.Delegate;

[PublicAPI]
public static class DelegateLog
{
    public static void Init(Action<LogLevel, string, string> logAction)
    {
        Ensure.IsNotNull(logAction, nameof(logAction));

        LogManager.SetLoggerFactory(new DelegateLoggerFactory(logAction));
    }

    public static bool IsTraceEnabled { get; set; } = true;

    public static bool IsDebugEnabled { get; set; } = true;

    public static bool IsInfoEnabled { get; set; } = true;

    public static bool IsWarnEnabled { get; set; } = true;

    public static bool IsErrorEnabled { get; set; } = true;

    public static bool IsFatalEnabled { get; set; } = true;
}