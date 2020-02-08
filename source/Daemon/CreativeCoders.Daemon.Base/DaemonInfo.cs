using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;

namespace CreativeCoders.Daemon.Base
{
    [PublicAPI]
    public class DaemonInfo
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool CanStop { get; set; }

        public bool CanPauseAndContinue { get; set; }

        public bool AutoLog { get; set; }

        [DefaultValue(DaemonAccount.LocalService)]
        public DaemonAccount Account { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        [DefaultValue(DaemonStartMode.Manual)]
        public DaemonStartMode StartMode { get; set; }

        public bool DelayedAutoStart { get; set; }

        public ICollection<string> DaemonsDependedOn { get; set; } = new List<string>();

        public string WorkingDirectory { get; set; }

        public string SyslogIdentifier { get; set; }

        public string Environment { get; set; }

        public ICollection<string> StartBeforeDaemons { get; set; } = new List<string>();

        public ICollection<string> StartAfterDaemons { get; set; } = new List<string>();
    }
}