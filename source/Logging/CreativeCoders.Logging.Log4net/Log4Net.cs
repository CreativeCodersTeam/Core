using System.IO;
using System.IO.Abstractions;
using System.Reflection;
using CreativeCoders.Core.IO;
using CreativeCoders.Core.Logging;
using JetBrains.Annotations;
using log4net.Config;
using log4net.Repository;

namespace CreativeCoders.Logging.Log4net
{
    [PublicAPI]
    public static class Log4Net
    {
        private static FileSystemWatcherBase ConfigFileSystemWatcher;

        public static void Init()
        {
            Init("log4net.config", true);
        }

        public static void Init(string configFileName)
        {
            Init(configFileName, true);
        }

        public static void Init(string configFileName, bool watchForConfigFileChange)
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
            var logRepository = log4net.LogManager.GetRepository(assembly);

            LoadConfig(configFileName, logRepository);

            LogManager.SetLoggerFactory(new Log4NetLoggerFactory());

            if (watchForConfigFileChange)
            {
                SetupFileWatcher(configFileName, logRepository);
            }
        }

        private static void SetupFileWatcher(string configFileName,
            ILoggerRepository logRepository)
        {
            ConfigFileSystemWatcher?.Dispose();

            if (!FileSys.File.Exists(configFileName))
            {
                return;
            }

            ConfigFileSystemWatcher = FileSys.CreateFileSystemWatcher(FileSys.Path.GetDirectoryName(configFileName));
            ConfigFileSystemWatcher.Filter = FileSys.Path.GetFileName(configFileName);
            ConfigFileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            ConfigFileSystemWatcher.Changed += (sender, args) => ConfigFileSystemWatcherOnChanged(configFileName, logRepository);
            ConfigFileSystemWatcher.EnableRaisingEvents = true;
        }

        private static void ConfigFileSystemWatcherOnChanged(string configFileName, ILoggerRepository logRepository)
        {
            LoadConfig(configFileName, logRepository);
        }

        private static void LoadConfig(string configFileName, ILoggerRepository logRepository)
        {
            if (!FileSys.File.Exists(configFileName))
            {
                return;
            }
            using (var configStream = FileSys.File.OpenRead(configFileName))
            {
                XmlConfigurator.Configure(logRepository, configStream);
            }
        }        
    }
}