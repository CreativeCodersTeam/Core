using CreativeCoders.Core;
using JetBrains.Annotations;

namespace CreativeCoders.Daemon.Base
{
    [PublicAPI]
    public abstract class DaemonBase : IDaemon
    {
        public abstract void OnStart();

        public abstract void OnStop();

        protected void SetNewState(DaemonState newState)
        {
            if (State == newState)
            {
                return;
            }

            State = newState;
            OnStateChanged(newState);
        }

        public DaemonState State { get; private set; }

        public event EventHandlerEx<IDaemon, DaemonState> StateChanged;

        protected virtual void OnStateChanged(DaemonState e)
        {
            StateChanged?.Invoke(this, e);
        }
    }
}