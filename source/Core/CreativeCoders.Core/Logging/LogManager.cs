using System;

namespace CreativeCoders.Core.Logging;

public static class LogManager
{
    private static ILoggerFactory LoggerFactory;

    public static void SetLoggerFactory(ILoggerFactory loggerFactory)
    {
        LoggerFactory = loggerFactory;
    }

    public static ILogger GetLogger(string scope)
    {
        return LoggerFactory?.GetLogger(scope) ?? new NullLogger();
    }

    public static ILogger GetLogger(Type scopeType)
    {
        return LoggerFactory?.GetLogger(scopeType) ?? new NullLogger();
    }

    public static ILogger GetLogger<TScope>()
    {
        return GetLogger(typeof(TScope));
    }
}