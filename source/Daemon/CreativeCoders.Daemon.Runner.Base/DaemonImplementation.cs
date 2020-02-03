using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core;
using CreativeCoders.Daemon.Base;

namespace CreativeCoders.Daemon.Runner.Base
{
    public abstract class DaemonImplementation<TRunner, TDaemon> where TRunner: DaemonRunnerBase<TDaemon>, new() where TDaemon: class, IDaemon, new()
    {
        private readonly string _installArg;

        private readonly string _uninstallArg;

        private readonly TRunner _runner;

        protected DaemonImplementation(string installArg, string uninstallArg)
        {
            Ensure.IsNotNullOrWhitespace(installArg, nameof(installArg));
            Ensure.IsNotNullOrWhitespace(uninstallArg, nameof(uninstallArg));

            _installArg = installArg;
            _uninstallArg = uninstallArg;
            _runner = new TRunner();
        }

        public void Run(IEnumerable<string> args)
        {
            var firstArg = args?.FirstOrDefault();
            if (firstArg == _installArg)
            {
                Install();
                return;
            }
            if (firstArg == _uninstallArg)
            {
                Uninstall();
                return;
            }

            _runner.Run();
        }

        protected abstract void Uninstall();

        protected abstract void Install();
    }
}