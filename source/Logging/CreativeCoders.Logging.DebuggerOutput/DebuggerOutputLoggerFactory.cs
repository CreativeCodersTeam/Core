using System;
using CreativeCoders.Core.Logging;

namespace CreativeCoders.Logging.DebuggerOutput;

internal class DebuggerOutputLoggerFactory : ILoggerFactory
{
    public ILogger GetLogger(string scope)
    {
        return new SimpleLoggerWrapper(new DebuggerOutputLogger(scope));
    }

    public ILogger GetLogger(Type scopeType)
    {
        return GetLogger(scopeType.FullName);
    }
}
