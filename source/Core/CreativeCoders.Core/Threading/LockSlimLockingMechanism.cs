using System;
using System.Threading;

namespace CreativeCoders.Core.Threading;

public class LockSlimLockingMechanism : IUpgradeableLockingMechanism
{
    private readonly ReaderWriterLockSlim _lock;

    public LockSlimLockingMechanism() : this(new ReaderWriterLockSlim()) { }

    public LockSlimLockingMechanism(ReaderWriterLockSlim lockSlim)
    {
        Ensure.IsNotNull(lockSlim, nameof(lockSlim));

        _lock = lockSlim;
    }

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
