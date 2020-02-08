using System;
using CreativeCoders.Core.Logging;

namespace CreativeCoders.Logging.Console
{
    internal class ConsoleLoggerFactory : ILoggerFactory
    {
        public ILogger GetLogger(string scope)
        {
            return new SimpleLoggerWrapper(new ConsoleLogger(scope));
        }

        public ILogger GetLogger(Type scopeType)
        {
            return GetLogger(scopeType.FullName);
        }
    }
}