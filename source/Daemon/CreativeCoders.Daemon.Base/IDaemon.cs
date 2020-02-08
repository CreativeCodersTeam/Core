using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Daemon.Base
{
    [PublicAPI]
    public interface IDaemon
    {
        void OnStart();

        void OnStop();

        DaemonState State { get; }

        event EventHandlerEx<IDaemon, DaemonState> StateChanged;
    }
}
