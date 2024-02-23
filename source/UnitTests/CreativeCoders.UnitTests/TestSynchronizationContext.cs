using System.Threading;
using JetBrains.Annotations;

namespace CreativeCoders.UnitTests;

[PublicAPI]
public class TestSynchronizationContext : SynchronizationContext
{
    public override void Post(SendOrPostCallback d, object? state)
    {
        d(state);
    }

    public override void Send(SendOrPostCallback d, object? state)
    {
        d(state);
    }
}
