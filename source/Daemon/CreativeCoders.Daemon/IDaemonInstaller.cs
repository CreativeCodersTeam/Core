using CreativeCoders.Daemon.Definition;

namespace CreativeCoders.Daemon;

public interface IDaemonInstaller
{
    void Install(DaemonDefinition daemonDefinition);

    void Uninstall(DaemonDefinition daemonDefinition);
}
