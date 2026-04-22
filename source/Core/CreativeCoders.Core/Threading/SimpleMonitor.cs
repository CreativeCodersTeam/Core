using System;
using System.Threading;

namespace CreativeCoders.Core.Threading;

/// <summary>
///     Provides a simple thread-safe reentrancy monitor. Call <see cref="Enter"/> to signal
///     entry and dispose the instance to signal exit. The <see cref="Busy"/> property indicates
///     whether the monitor is currently entered.
/// </summary>
public sealed class SimpleMonitor : IDisposable
{
    private long _counter;

    /// <summary>
    ///     Signals entry into the monitored region by incrementing the internal counter.
    /// </summary>
    public void Enter()
    {
        Interlocked.Increment(ref _counter);
    }

    /// <summary>
    ///     Signals exit from the monitored region by decrementing the internal counter.
    /// </summary>
    public void Dispose()
    {
        Interlocked.Decrement(ref _counter);
    }

    /// <summary>
    ///     Gets a value indicating whether the monitor is currently entered.
    /// </summary>
    public bool Busy => Interlocked.Read(ref _counter) > 0;
}
