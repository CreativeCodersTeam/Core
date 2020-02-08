using System;
using System.Threading;

namespace CreativeCoders.Core.Threading
{
    public class LockSlimLockingMechanism : ILockingMechanism
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

        public T Read<T>(Func<T> func)
        {
            _lock.EnterReadLock();
            try
            {
                return func();
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

        public T Write<T>(Func<T> func)
        {
            _lock.EnterWriteLock();
            try
            {
                return func();
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }
    }
}
