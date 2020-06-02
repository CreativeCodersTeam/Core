using System;
using JetBrains.Annotations;

namespace CreativeCoders.Daemon.Base.Info
{
    /// <summary>   Information about the daemon. </summary>
    [PublicAPI]
    public class DaemonInfo
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the name. </summary>
        ///
        /// <value> The name of the daemon. </value>
        ///-------------------------------------------------------------------------------------------------
        public string Name { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the display name. </summary>
        ///
        /// <value> The display name of the daemon. </value>
        ///-------------------------------------------------------------------------------------------------
        public string DisplayName { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the description. </summary>
        ///
        /// <value> The description of the daemon. </value>
        ///-------------------------------------------------------------------------------------------------
        public string Description { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the arguments. </summary>
        ///
        /// <value> The arguments passed to the daemon application via command line arguments. </value>
        ///-------------------------------------------------------------------------------------------------
        public string[] Arguments { get; set; } = Array.Empty<string>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the account. </summary>
        ///
        /// <value> The account used for daemon service. </value>
        ///-------------------------------------------------------------------------------------------------
        public DaemonAccount Account { get; set; } = DaemonAccount.LocalSystem;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the user. </summary>
        ///
        /// <value> The user used for daemon service. </value>
        ///-------------------------------------------------------------------------------------------------
        public string User { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the password. </summary>
        ///
        /// <value> The password for the user used for daemon service. </value>
        ///-------------------------------------------------------------------------------------------------
        public string Password { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the start mode. </summary>
        ///
        /// <value> The start mode of the daemon service. </value>
        ///-------------------------------------------------------------------------------------------------
        public DaemonStartMode StartMode { get; set; } = DaemonStartMode.Manual;

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets a value indicating whether the automatic start is delayed. </summary>
        ///
        /// <value> True if automatic delayed start is active, false if not. </value>
        ///-------------------------------------------------------------------------------------------------
        public bool DelayedAutoStart { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the daemons this daemon depends on. </summary>
        ///
        /// <value> The daemons this daemon depends on. </value>
        ///-------------------------------------------------------------------------------------------------
        public string[] DaemonsDependedOn { get; set; } = Array.Empty<string>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the working directory. </summary>
        ///
        /// <value> The working directory for the daemon service. Is only used on systemd. </value>
        ///-------------------------------------------------------------------------------------------------
        public string WorkingDirectory { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the identifier for syslog. </summary>
        ///
        /// <value> The identifier for syslog. Is only used on systemd. </value>
        ///-------------------------------------------------------------------------------------------------
        public string SyslogIdentifier { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the environment. </summary>
        ///
        /// <value> The environment. Is only used on systemd. </value>
        ///-------------------------------------------------------------------------------------------------
        public string Environment { get; set; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets daemons to start before this daemon gets started. </summary>
        ///
        /// <value> The daemons to start before this daemon. </value>
        ///-------------------------------------------------------------------------------------------------
        public string[] StartBeforeDaemons { get; set; } = Array.Empty<string>();

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets daemons to start after this daemon gets started. </summary>
        ///
        /// <value> The daemons to start after this daemon. </value>
        ///-------------------------------------------------------------------------------------------------
        public string[] StartAfterDaemons { get; set; } = Array.Empty<string>();
    }
}