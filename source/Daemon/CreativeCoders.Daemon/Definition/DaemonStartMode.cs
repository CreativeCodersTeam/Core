namespace CreativeCoders.Daemon.Definition;

/// <summary> Indicates the start mode of the daemon. </summary>
public enum DaemonStartMode
{
    /// <summary>
    /// Indicates that the service is a device driver started by the system. Only valid for device drivers.
    /// </summary>
    Boot,

    /// <summary>
    /// Indicates that the service is a device driver started by the IOInitSystem function.
    /// Only valid for device drivers.
    /// </summary>
    System,

    /// <summary>
    /// Indicates that the service gets started by the operating system, at system startup.
    /// If an automatically started service depends on a manually started service,
    /// the manually started service is also started automatically at system startup.
    /// </summary>
    Automatic,

    /// <summary>
    /// Indicates that the service is started only manually, by a user or by an application.
    /// </summary>
    Manual,

    /// <summary>
    /// Indicates that the service is disabled, so that it cannot be started by a user or application.
    /// </summary>
    Disabled
}
