using CreativeCoders.Daemon.Base;

namespace CreativeCoders.Daemon.Runner.Base
{
    // ReSharper disable once UnusedTypeParameter
    public abstract class DaemonRunnerBase<TDaemon>
        where TDaemon: class, IDaemon, new()
    {
        public abstract void Run();
    }
}