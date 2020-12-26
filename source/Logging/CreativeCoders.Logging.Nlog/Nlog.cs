using System.IO;
using System.IO.Abstractions;
using System.Xml.Linq;
using CreativeCoders.Core.IO;
using CreativeCoders.Core.Logging;
using JetBrains.Annotations;
using NLog.Config;

namespace CreativeCoders.Logging.Nlog
{
    [PublicAPI]
    public static class Nlog
    {
        private static NlogLoggerFactory NlogLoggerFactoryInstance;

        private static FileSystemWatcherBase ConfigFileSystemWatcher;

        public static void Init()
        {
            Init("NLog.config", true);
        }

        public static void Init(string configFileName)
        {
            Init(configFileName, true);
        }

        public static void Init(string configFileName, bool watchForConfigFileChange)
        {
            var config = LoadConfig(configFileName);

            Init(config);

            if (watchForConfigFileChange)
            {
                SetupFileWatcher(configFileName);
            }
        }

        private static void SetupFileWatcher(string configFileName)
        {
            ConfigFileSystemWatcher?.Dispose();

            if (!FileSys.File.Exists(configFileName))
            {
                return;
            }

            ConfigFileSystemWatcher = FileSys.CreateFileSystemWatcher(FileSys.Path.GetDirectoryName(configFileName));
            ConfigFileSystemWatcher.Filter = FileSys.Path.GetFileName(configFileName);
            ConfigFileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            ConfigFileSystemWatcher.Changed += (_, _) => ConfigFileSystemWatcherOnChanged(configFileName);
            ConfigFileSystemWatcher.EnableRaisingEvents = true;
        }

        private static void ConfigFileSystemWatcherOnChanged(string configFileName)
        {
            NlogLoggerFactory.SetConfiguration(LoadConfig(configFileName));
        }

        private static XmlLoggingConfiguration LoadConfig(string configFileName)
        {
            if (!FileSys.File.Exists(configFileName))
            {
                return null;
            }
            var element = XElement.Parse(FileSys.File.ReadAllText(configFileName));
            return new XmlLoggingConfiguration(element.CreateReader(), null);
        }

        public static void Init(LoggingConfiguration configuration)
        {
            NlogLoggerFactoryInstance = new NlogLoggerFactory(configuration);
            LogManager.SetLoggerFactory(NlogLoggerFactoryInstance);
        }
    }
}
