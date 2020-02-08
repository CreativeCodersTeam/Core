using System;
using CreativeCoders.Daemon.Base;
using CreativeCoders.Daemon.Runner.Base;
using JetBrains.Annotations;

namespace CreativeCoders.Daemon.Linux
{
    [PublicAPI]
    public class LinuxDaemonImplementation<TDaemon> : DaemonImplementation<LinuxDaemonRunner<TDaemon>, TDaemon> where TDaemon : class, IDaemon, new()
    {
        public LinuxDaemonImplementation(string installArg, string uninstallArg) : base(installArg, uninstallArg)
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
            {
                throw new PlatformNotSupportedException("Daemon only supports Linux");
            }
        }

        protected override void Uninstall()
        {
            new SystemdDaemonInstaller().Uninstall();
        }

        protected override void Install()
        {
            new SystemdDaemonInstaller().Install();
        }
    }
}