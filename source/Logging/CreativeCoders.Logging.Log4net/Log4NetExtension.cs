using System;
using System.Globalization;
using System.Reflection;
using JetBrains.Annotations;
using log4net;
using log4net.Core;
using log4net.Util;

namespace CreativeCoders.Logging.Log4net;

[PublicAPI]
public static class Log4NetExtension
{
    public static void Log(this ILog log, Level level, object message, Exception exception)
    {
        if (log.Logger.IsEnabledFor(level))
        {
            log.Logger.Log(GetDeclaringType() ?? typeof(void), level, message, exception);
        }
    }

    public static void Log(this ILog log, Level level, object message)
    {
        log.Log(level, message, null);
    }

    public static void LogFormat(this ILog log, Level level, string format, params object[] args)
    {
        if (log.Logger.IsEnabledFor(level))
        {
            log.Log(level, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
        }
    }

    public static bool IsTraceEnabled(this ILog log)
    {
        return log.Logger.IsEnabledFor(Level.Trace);
    }

    public static void Trace(this ILog log, object message, Exception exception)
    {
        log.Log(Level.Trace, message, exception);
    }

    public static void Trace(this ILog log, object message)
    {
        log.Trace(message, null);
    }

    public static void TraceFormat(this ILog log, string format, params object[] args)
    {
        if (log.IsTraceEnabled())
        {
            log.Log(Level.Trace, new SystemStringFormat(CultureInfo.InvariantCulture, format, args), null);
        }
    }

    private static Type GetDeclaringType()
    {
        return MethodBase.GetCurrentMethod()?.DeclaringType;
    }
}
