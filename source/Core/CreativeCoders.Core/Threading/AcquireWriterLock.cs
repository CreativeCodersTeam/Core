using System;
using System.Threading;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Threading
{
    [PublicAPI]
    public class AcquireWriterLock : IDisposable
    {
        private readonly ReaderWriterLockSlim _lockSlim;

        private bool _disposed;

        public AcquireWriterLock(ReaderWriterLockSlim lockSlim) : this(lockSlim, Timeout.Infinite) {}

        public AcquireWriterLock(ReaderWriterLockSlim lockSlim, int timeout)
        {
            Ensure.IsNotNull(lockSlim, nameof(lockSlim));

            _lockSlim = lockSlim;
            if (!_lockSlim.TryEnterWriteLock(timeout))
            {
                throw new AcquireLockFailedException("Acquire writer lock failed", timeout);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _lockSlim.ExitWriteLock();
            }
            _disposed = true;
        }
    }
}