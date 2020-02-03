using System.ServiceProcess;
using CreativeCoders.Core;
using CreativeCoders.Daemon.Base;

namespace CreativeCoders.Daemon.Windows
{
    public class WinService : ServiceBase
    {
        private readonly IDaemon _daemon;

        private readonly DaemonInfo _daemonInfo;

        public WinService(IDaemon daemon, DaemonInfo daemonInfo)
        {
            Ensure.IsNotNull(daemon, nameof(daemon));
            Ensure.IsNotNull(daemonInfo, nameof(daemonInfo));

            _daemon = daemon;
            _daemonInfo = daemonInfo;

            Init();
        }

        private void Init()
        {
            ServiceName = _daemonInfo.Name;
            AutoLog = _daemonInfo.AutoLog;
            CanStop = _daemonInfo.CanStop;
            CanPauseAndContinue = _daemonInfo.CanPauseAndContinue;
        }

        protected override void OnStart(string[] args)
        {
            _daemon.OnStart();
        }

        protected override void OnStop()
        {
            _daemon.OnStop();
        }
    }
}