using System.Reflection;
using System.ServiceProcess;
using CreativeCoders.Core.IO;
using CreativeCoders.Daemon.Base;
using CreativeCoders.Daemon.Runner.Base;

namespace CreativeCoders.Daemon.Windows
{
    public class WindowsDaemonRunner<TDaemon> : DaemonRunnerBase<TDaemon> where TDaemon : class, IDaemon, new()
    {
        private readonly WinService _service;

        public WindowsDaemonRunner()
        {
            var daemonInfo = LoadDaemonInfo();
            var daemon = new TDaemon();
            _service = new WinService(daemon, daemonInfo);
        }

        private static DaemonInfo LoadDaemonInfo()
        {
            var path = FileSys.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? string.Empty;
            var fileName = FileSys.Path.Combine(path, "daemon.json");
            var infoFile = new DaemonInfoFile(fileName);
            return infoFile.LoadInfo();
        }

        public override void Run()
        {
            ServiceBase.Run(_service);
        }
    }
}