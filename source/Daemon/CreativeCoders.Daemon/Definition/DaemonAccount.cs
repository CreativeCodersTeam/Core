namespace CreativeCoders.Daemon.Definition;

/// <summary> Values that represent user accounts used for worker execution. </summary>
public enum DaemonAccount
{
    /// <summary> Run as NT AUTHORITY\LocalService. </summary>
    LocalService,

    /// <summary> Run as NT AUTHORITY\NetworkService. </summary>
    NetworkService,

    /// <summary> Run as NT AUTHORITY\LocalSystem. </summary>
    LocalSystem,

    /// <summary> Run as user specified by <see cref="DaemonDefinition.User"/>. </summary>
    User
}
