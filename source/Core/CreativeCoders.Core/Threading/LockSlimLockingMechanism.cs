using System;
using System.Threading;

namespace CreativeCoders.Core.Threading;

/// <summary>
///     Provides a locking mechanism based on <see cref="ReaderWriterLockSlim"/>
///     that supports concurrent reads, exclusive writes, and upgradeable read locks.
/// </summary>
public class LockSlimLockingMechanism : IUpgradeableLockingMechanism
{
    private readonly ReaderWriterLockSlim _lock;

    /// <summary>
    ///     Initializes a new instance of the <see cref="LockSlimLockingMechanism"/> class
    ///     with a new <see cref="ReaderWriterLockSlim"/>.
    /// </summary>
    public LockSlimLockingMechanism() : this(new ReaderWriterLockSlim()) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="LockSlimLockingMechanism"/> class
    ///     with the specified <see cref="ReaderWriterLockSlim"/>.
    /// </summary>
    /// <param name="lockSlim">The reader-writer lock to use.</param>
    public LockSlimLockingMechanism(ReaderWriterLockSlim lockSlim)
    {
        Ensure.IsNotNull(lockSlim);

        _lock = lockSlim;
    }

    /// <inheritdoc />
    public void Read(Action action)
    {
        _lock.EnterReadLock();
        try
        {
            action();
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <inheritdoc />
    public T Read<T>(Func<T> function)
    {
        _lock.EnterReadLock();
        try
        {
            return function();
        }
        finally
        {
            _lock.ExitReadLock();
        }
    }

    /// <inheritdoc />
    public void Write(Action action)
    {
        _lock.EnterWriteLock();
        try
        {
            action();
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    public T Write<T>(Func<T> function)
    {
        _lock.EnterWriteLock();
        try
        {
            return function();
        }
        finally
        {
            _lock.ExitWriteLock();
        }
    }

    /// <inheritdoc />
    public void UpgradeableRead(UpgradeableReadAction action)
    {
        _lock.EnterUpgradeableReadLock();
        try
        {
            action(
                () =>
                {
                    _lock.EnterWriteLock();

                    return new DelegateDisposable(() => _lock.ExitWriteLock(), true);
                }
            );
        }
        finally
        {
            _lock.ExitUpgradeableReadLock();
        }
    }

    /// <inheritdoc />
    public T UpgradeableRead<T>(UpgradeableReadFunc<T> function)
    {
        _lock.EnterUpgradeableReadLock();
        try
        {
            return function(
                () =>
                {
                    _lock.EnterWriteLock();

                    return new DelegateDisposable(() => _lock.ExitWriteLock(), true);
                }
            );
        }
        finally
        {
            _lock.ExitUpgradeableReadLock();
        }
    }
}
