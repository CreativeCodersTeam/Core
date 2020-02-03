using System;

namespace CreativeCoders.Core.Logging
{
    public interface ILoggerFactory
    {
        ILogger GetLogger(string scope);

        ILogger GetLogger(Type scopeType);
    }
}