using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Threading;

[PublicAPI]
public class AcquireUpgradeableReaderLock : IDisposable
{
    private readonly ReaderWriterLockSlim _lockSlim;
        
    private bool _disposed;

    [ExcludeFromCodeCoverage]
    public AcquireUpgradeableReaderLock() : this(new ReaderWriterLockSlim()) { }

    public AcquireUpgradeableReaderLock(ReaderWriterLockSlim lockSlim) : this(lockSlim, Timeout.Infinite) { }

    public AcquireUpgradeableReaderLock(ReaderWriterLockSlim lockSlim, int timeout)
    {
        Ensure.IsNotNull(lockSlim, nameof(lockSlim));

        _lockSlim = lockSlim;
        if (!_lockSlim.TryEnterUpgradeableReadLock(timeout))
        {
            throw new AcquireLockFailedException("Acquire upgradeable reader lock failed", timeout);
        }
    }

    public IDisposable UseWriteLock()
    {
        return UseWriteLock(Timeout.Infinite);
    }
        
    public IDisposable UseWriteLock(int timeout)
    {
        if (!_lockSlim.TryEnterWriteLock(timeout))
        {
            throw new AcquireLockFailedException("Acquire upgraded writer lock failed", timeout);
        }
            
        return new DelegateDisposable(() => _lockSlim.ExitWriteLock(), true);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _lockSlim.ExitUpgradeableReadLock();
        }
        _disposed = true;
    }
}