using System.Threading;

namespace CreativeCoders.UnitTests
{
    public class TestSynchronizationContext : SynchronizationContext
    {
        public override void Post(SendOrPostCallback callback, object state)
        {
            callback(state);
        }

        public override void Send(SendOrPostCallback callback, object state)
        {
            callback(state);
        }
    }
}