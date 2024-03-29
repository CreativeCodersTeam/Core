﻿using System;
using System.Threading;

namespace CreativeCoders.Core.Threading;

public sealed class SimpleMonitor : IDisposable
{
    private long _counter;

    public void Enter()
    {
        Interlocked.Increment(ref _counter);
    }

    public void Dispose()
    {
        Interlocked.Decrement(ref _counter);
    }

    public bool Busy => Interlocked.Read(ref _counter) > 0;
}
