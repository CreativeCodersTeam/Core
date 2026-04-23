using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Threading;

/// <summary>
///     Acquires a write lock on a <see cref="ReaderWriterLockSlim"/> upon construction
///     and releases it upon disposal.
/// </summary>
[PublicAPI]
public sealed class AcquireWriterLock : IDisposable
{
    private readonly ReaderWriterLockSlim _lockSlim;

    private bool _disposed;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AcquireWriterLock"/> class
    ///     with a new <see cref="ReaderWriterLockSlim"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public AcquireWriterLock() : this(new ReaderWriterLockSlim()) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="AcquireWriterLock"/> class
    ///     with the specified lock and an infinite timeout.
    /// </summary>
    /// <param name="lockSlim">The reader-writer lock to acquire a write lock on.</param>
    public AcquireWriterLock(ReaderWriterLockSlim lockSlim) : this(lockSlim, Timeout.Infinite) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="AcquireWriterLock"/> class
    ///     with the specified lock and timeout.
    /// </summary>
    /// <param name="lockSlim">The reader-writer lock to acquire a write lock on.</param>
    /// <param name="timeout">The timeout in milliseconds for acquiring the lock.</param>
    /// <exception cref="AcquireLockFailedException">The write lock could not be acquired within the specified timeout.</exception>
    public AcquireWriterLock(ReaderWriterLockSlim lockSlim, int timeout)
    {
        Ensure.IsNotNull(lockSlim);

        _lockSlim = lockSlim;

        if (!_lockSlim.TryEnterWriteLock(timeout))
        {
            throw new AcquireLockFailedException("Acquire writer lock failed", timeout);
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Releases the write lock if it has not already been released.
    /// </summary>
    /// <param name="disposing">
    ///     <see langword="true"/> to release managed resources; otherwise, <see langword="false"/>.
    /// </param>
    protected void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            _lockSlim.ExitWriteLock();
        }

        _disposed = true;
    }
}
