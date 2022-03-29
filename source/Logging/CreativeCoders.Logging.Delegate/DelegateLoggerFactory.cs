using System;
using CreativeCoders.Core.Logging;

namespace CreativeCoders.Logging.Delegate;

public class DelegateLoggerFactory : ILoggerFactory
{
    private readonly Action<LogLevel, string, string> _logAction;

    public DelegateLoggerFactory(Action<LogLevel, string, string> logAction)
    {
        _logAction = logAction;
    }

    public ILogger GetLogger(string scope)
    {
        return new SimpleLoggerWrapper(new DelegateLogger(scope, _logAction));
    }

    public ILogger GetLogger(Type scopeType)
    {
        return GetLogger(scopeType.FullName);
    }
}
