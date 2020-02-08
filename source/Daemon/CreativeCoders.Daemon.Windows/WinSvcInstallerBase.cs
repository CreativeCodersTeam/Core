using System;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using CreativeCoders.Daemon.Base;
using JetBrains.Annotations;

namespace CreativeCoders.Daemon.Windows
{
    [PublicAPI]
    public class WinSvcInstallerBase : Installer
    {
        private readonly ServiceProcessInstaller _processInstaller;

        private readonly ServiceInstaller _serviceInstaller;

        private readonly DaemonInfo _info;

        public WinSvcInstallerBase()
        {
            var instanceFile = new DaemonInfoFile("daemon.json");
            _info = instanceFile.LoadInfo();

            _processInstaller = new ServiceProcessInstaller();
            _serviceInstaller = new ServiceInstaller();

            InitInstallers();

            Installers.Add(_processInstaller);
            Installers.Add(_serviceInstaller);
        }

        private void InitInstallers()
        {
            _processInstaller.Account = GetServiceAccount(_info.Account);
            _serviceInstaller.ServiceName = _info.Name;
            _serviceInstaller.Description = _info.Description;
            _serviceInstaller.StartType = GetStartType(_info.StartMode);
            _serviceInstaller.DelayedAutoStart = _info.DelayedAutoStart;
            _serviceInstaller.ServicesDependedOn = _info.DaemonsDependedOn.ToArray();
        }

        private ServiceAccount GetServiceAccount(DaemonAccount account)
        {
            switch (account)
            {
                case DaemonAccount.LocalService:
                    return ServiceAccount.LocalService;
                case DaemonAccount.NetworkService:
                    return ServiceAccount.NetworkService;
                case DaemonAccount.LocalSystem:
                    return ServiceAccount.LocalSystem;
                case DaemonAccount.User:
                    return ServiceAccount.User;
                default:
                    throw new ArgumentOutOfRangeException(nameof(account), account, null);
            }
        }

        private static ServiceStartMode GetStartType(DaemonStartMode startMode)
        {
            switch (startMode)
            {
                case DaemonStartMode.Boot:
                    return ServiceStartMode.Boot;
                case DaemonStartMode.System:
                    return ServiceStartMode.System;
                case DaemonStartMode.Automatic:
                    return ServiceStartMode.Automatic;
                case DaemonStartMode.Manual:
                    return ServiceStartMode.Manual;
                case DaemonStartMode.Disabled:
                    return ServiceStartMode.Disabled;
                default:
                    return ServiceStartMode.Disabled;
            }
        }
    }
}