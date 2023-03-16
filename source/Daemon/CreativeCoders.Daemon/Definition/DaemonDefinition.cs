using JetBrains.Annotations;

namespace CreativeCoders.Daemon.Definition;

/// <summary> Information about the daemon. </summary>
[PublicAPI]
public class DaemonDefinition
{
    /// <summary> Gets or sets the name of the daemon. </summary>
    public string Name { get; set; }

    /// <summary> Gets or sets the display name of the daemon. </summary>
    public string DisplayName { get; set; }

    /// <summary> Gets or sets the description. </summary>
    public string Description { get; set; }

    /// <summary> Gets or sets the arguments. </summary>
    public string[] Arguments { get; set; } = Array.Empty<string>();

    /// <summary> Gets or sets the account. </summary>
    public DaemonAccount Account { get; set; } = DaemonAccount.LocalSystem;

    /// <summary> Gets or sets the user. </summary>
    public string User { get; set; }

    /// <summary> Gets or sets the password. </summary>
    public string Password { get; set; }

    /// <summary> Gets or sets the start mode. </summary>
    public DaemonStartMode StartMode { get; set; } = DaemonStartMode.Manual;

    /// <summary> Gets or sets a value indicating whether the automatic start is delayed. </summary>
    public bool DelayedAutoStart { get; set; }

    /// <summary> Gets or sets the daemons this daemon depends on. </summary>
    public string[] DaemonsDependedOn { get; set; } = Array.Empty<string>();

    /// <summary> Gets or sets the working directory. </summary>
    public string WorkingDirectory { get; set; }

    /// <summary> Gets or sets the identifier for syslog. </summary>
    public string SyslogIdentifier { get; set; }

    /// <summary> Gets or sets the environment. </summary>
    public string Environment { get; set; }

    /// <summary> Gets or sets daemons to start before this daemon gets started. </summary>
    public string[] StartBeforeDaemons { get; set; } = Array.Empty<string>();

    /// <summary> Gets or sets daemons to start after this daemon gets started. </summary>
    public string[] StartAfterDaemons { get; set; } = Array.Empty<string>();
}
