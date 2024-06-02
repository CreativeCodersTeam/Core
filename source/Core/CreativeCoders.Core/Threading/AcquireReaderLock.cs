using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Threading;

[PublicAPI]
public sealed class AcquireReaderLock : IDisposable
{
    private readonly ReaderWriterLockSlim _lockSlim;

    private bool _disposed;

    [ExcludeFromCodeCoverage]
    public AcquireReaderLock() : this(new ReaderWriterLockSlim()) { }

    public AcquireReaderLock(ReaderWriterLockSlim lockSlim) : this(lockSlim, Timeout.Infinite) { }

    public AcquireReaderLock(ReaderWriterLockSlim lockSlim, int timeout)
    {
        Ensure.IsNotNull(lockSlim);

        _lockSlim = lockSlim;
        try
        {
            if (!_lockSlim.TryEnterReadLock(timeout))
            {
                throw new AcquireLockFailedException("Acquire reader lock failed", timeout);
            }
        }
        catch (LockRecursionException e)
        {
            throw new AcquireLockFailedException("Acquire reader lock failed (Lock recursion)", timeout, e);
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _lockSlim.ExitReadLock();
        }

        _disposed = true;
    }
}
