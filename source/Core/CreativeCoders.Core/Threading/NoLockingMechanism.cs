using System;

namespace CreativeCoders.Core.Threading;

/// <summary>
///     Provides a no-op locking mechanism that executes actions and functions without any
///     synchronization. Suitable for single-threaded environments.
/// </summary>
public class NoLockingMechanism : ILockingMechanism
{
    /// <inheritdoc />
    public void Read(Action action)
    {
        action();
    }

    /// <inheritdoc />
    public T Read<T>(Func<T> function)
    {
        return function();
    }

    /// <inheritdoc />
    public void Write(Action action)
    {
        action();
    }

    /// <inheritdoc />
    public T Write<T>(Func<T> function)
    {
        return function();
    }
}
