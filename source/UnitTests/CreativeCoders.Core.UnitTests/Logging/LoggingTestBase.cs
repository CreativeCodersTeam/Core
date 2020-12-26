using System;
using CreativeCoders.Core.Logging;

namespace CreativeCoders.Core.UnitTests.Logging
{
    public class LoggingTestBase
    {
        protected static void CallLogger(ILogger logger)
        {
            logger.Debug(1234);
            logger.Debug("test");
            logger.Debug("test1", new Exception());
            logger.Debug(() => 5678);

            logger.DebugFormat("{0}:{1}", "test2", 1290);

            logger.Error(1234);
            logger.Error("test");
            logger.Error("test1", new Exception());
            logger.Error(() => 5678);

            logger.ErrorFormat("{0}:{1}", "test2", 1290);

            logger.Fatal(1234);
            logger.Fatal("test");
            logger.Fatal("test1", new Exception());
            logger.Fatal(() => 5678);

            logger.FatalFormat("{0}:{1}", "test2", 1290);

            logger.Info(1234);
            logger.Info("test");
            logger.Info("test1", new Exception());
            logger.Info(() => 5678);

            logger.InfoFormat("{0}:{1}", "test2", 1290);

            logger.Trace(1234);
            logger.Trace("test");
            logger.Trace("test1", new Exception());
            logger.Trace(() => 5678);

            logger.TraceFormat("{0}:{1}", "test2", 1290);

            logger.Warn(1234);
            logger.Warn("test");
            logger.Warn("test1", new Exception());
            logger.Warn(() => 5678);

            logger.WarnFormat("{0}:{1}", "test2", 1290);

            CallLogForLevels(logger, LogLevel.Fatal, LogLevel.Debug, LogLevel.Error, LogLevel.Info, LogLevel.Trace, LogLevel.Warn);

            logger.LogFormat(LogLevel.Info, "{0}:{1}", "test2", 1290);

            logger.Log((LogLevel)(-1), "test");
        }

        private static void CallLogForLevels(ILogger logger, params LogLevel[] logLevels)
        {
            foreach (var logLevel in logLevels)
            {
                logger.Log(logLevel, 1234);
                logger.Log(logLevel, "test");
                logger.Log(logLevel, "test1", new Exception());
                logger.Log(logLevel, () => 5678);
            }
        }
    }
}