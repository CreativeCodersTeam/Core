using System;
using System.Configuration.Install;
using System.Reflection;
using CreativeCoders.Daemon.Base;
using CreativeCoders.Daemon.Runner.Base;
using JetBrains.Annotations;

namespace CreativeCoders.Daemon.Windows
{
    [PublicAPI]
    public class WindowsDaemonImplementation<TDaemon> : DaemonImplementation<WindowsDaemonRunner<TDaemon>, TDaemon> where TDaemon : class, IDaemon, new()
    {
        public WindowsDaemonImplementation(string installArg, string uninstallArg) : base(installArg, uninstallArg)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            {
                throw new PlatformNotSupportedException("Windows daemon only supports Windows");
            }
        }

        protected override void Uninstall()
        {
            ManagedInstallerClass.InstallHelper(new[] { "/u", Assembly.GetEntryAssembly()?.Location ?? Environment.CurrentDirectory });
        }

        protected override void Install()
        {
            ManagedInstallerClass.InstallHelper(new[] { Assembly.GetEntryAssembly()?.Location ?? Environment.CurrentDirectory });
        }
    }
}