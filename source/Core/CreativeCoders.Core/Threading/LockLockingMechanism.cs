using System;

namespace CreativeCoders.Core.Threading;

/// <summary>
///     Provides a locking mechanism based on the C# <see langword="lock"/> statement,
///     where both read and write operations use the same exclusive lock.
/// </summary>
public class LockLockingMechanism : ILockingMechanism
{
    private readonly object _lockObj;

    /// <summary>
    ///     Initializes a new instance of the <see cref="LockLockingMechanism"/> class
    ///     using the instance itself as the lock object.
    /// </summary>
    public LockLockingMechanism()
    {
        _lockObj = this;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="LockLockingMechanism"/> class
    ///     with the specified lock object.
    /// </summary>
    /// <param name="lockObject">The object to use for locking.</param>
    public LockLockingMechanism(object lockObject)
    {
        Ensure.IsNotNull(lockObject);

        _lockObj = lockObject;
    }

    /// <inheritdoc />
    public void Read(Action action)
    {
        lock (_lockObj)
        {
            action();
        }
    }

    /// <inheritdoc />
    public T Read<T>(Func<T> function)
    {
        lock (_lockObj)
        {
            return function();
        }
    }

    /// <inheritdoc />
    public void Write(Action action)
    {
        lock (_lockObj)
        {
            action();
        }
    }

    /// <inheritdoc />
    public T Write<T>(Func<T> function)
    {
        lock (_lockObj)
        {
            return function();
        }
    }
}
