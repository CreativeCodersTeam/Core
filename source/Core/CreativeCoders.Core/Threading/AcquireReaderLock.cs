using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Threading;

/// <summary>
///     Acquires a read lock on a <see cref="ReaderWriterLockSlim"/> upon construction
///     and releases it upon disposal.
/// </summary>
[PublicAPI]
public sealed class AcquireReaderLock : IDisposable
{
    private readonly ReaderWriterLockSlim _lockSlim;

    private bool _disposed;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AcquireReaderLock"/> class
    ///     with a new <see cref="ReaderWriterLockSlim"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public AcquireReaderLock() : this(new ReaderWriterLockSlim()) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="AcquireReaderLock"/> class
    ///     with the specified lock and an infinite timeout.
    /// </summary>
    /// <param name="lockSlim">The reader-writer lock to acquire a read lock on.</param>
    public AcquireReaderLock(ReaderWriterLockSlim lockSlim) : this(lockSlim, Timeout.Infinite) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="AcquireReaderLock"/> class
    ///     with the specified lock and timeout.
    /// </summary>
    /// <param name="lockSlim">The reader-writer lock to acquire a read lock on.</param>
    /// <param name="timeout">The timeout in milliseconds for acquiring the lock.</param>
    /// <exception cref="AcquireLockFailedException">The read lock could not be acquired within the specified timeout or due to a lock recursion violation.</exception>
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

    /// <inheritdoc />
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
