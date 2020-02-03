using System;
using CreativeCoders.Core.Logging;
using NLog.Config;

namespace CreativeCoders.Logging.Nlog
{
    internal class NlogLoggerFactory : ILoggerFactory
    {
        public NlogLoggerFactory(LoggingConfiguration config)
        {
            SetConfiguration(config);
        }

        public ILogger GetLogger(string scope)
        {
            var nlogLogger = NLog.LogManager.GetLogger(scope);
            return new NlogLogger(nlogLogger);
        }

        public ILogger GetLogger(Type scopeType)
        {
            var nlogLogger = NLog.LogManager.GetLogger(scopeType.FullName);
            return new NlogLogger(nlogLogger);
        }

        public static void SetConfiguration(LoggingConfiguration config)
        {
            if (config != null)
            {
                NLog.LogManager.Configuration = config;
            }
        }
    }
}