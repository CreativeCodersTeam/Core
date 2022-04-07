using System;
using System.Reflection;
using CreativeCoders.Core.Logging;

namespace CreativeCoders.Logging.Log4net;

internal class Log4NetLoggerFactory : ILoggerFactory
{
    public ILogger GetLogger(string scope)
    {
        var log4NetLog = log4net.LogManager.GetLogger(Assembly.GetCallingAssembly(), scope);
        return new Log4NetLogger(log4NetLog);
    }

    public ILogger GetLogger(Type scopeType)
    {
        var log4NetLog = log4net.LogManager.GetLogger(scopeType);
        return new Log4NetLogger(log4NetLog);
    }
}
