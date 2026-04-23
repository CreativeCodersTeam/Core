using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Threading;

/// <summary>
///     Acquires an upgradeable read lock on a <see cref="ReaderWriterLockSlim"/> upon construction
///     and releases it upon disposal. Supports upgrading to a write lock via <see cref="UseWriteLock()"/>.
/// </summary>
[PublicAPI]
public sealed class AcquireUpgradeableReaderLock : IDisposable
{
    private readonly ReaderWriterLockSlim _lockSlim;

    private bool _disposed;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AcquireUpgradeableReaderLock"/> class
    ///     with a new <see cref="ReaderWriterLockSlim"/>.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public AcquireUpgradeableReaderLock() : this(new ReaderWriterLockSlim()) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="AcquireUpgradeableReaderLock"/> class
    ///     with the specified lock and an infinite timeout.
    /// </summary>
    /// <param name="lockSlim">The reader-writer lock to acquire an upgradeable read lock on.</param>
    public AcquireUpgradeableReaderLock(ReaderWriterLockSlim lockSlim) : this(lockSlim, Timeout.Infinite) { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="AcquireUpgradeableReaderLock"/> class
    ///     with the specified lock and timeout.
    /// </summary>
    /// <param name="lockSlim">The reader-writer lock to acquire an upgradeable read lock on.</param>
    /// <param name="timeout">The timeout in milliseconds for acquiring the lock.</param>
    /// <exception cref="AcquireLockFailedException">The upgradeable read lock could not be acquired within the specified timeout.</exception>
    public AcquireUpgradeableReaderLock(ReaderWriterLockSlim lockSlim, int timeout)
    {
        Ensure.IsNotNull(lockSlim);

        _lockSlim = lockSlim;
        if (!_lockSlim.TryEnterUpgradeableReadLock(timeout))
        {
            throw new AcquireLockFailedException("Acquire upgradeable reader lock failed", timeout);
        }
    }

    /// <summary>
    ///     Upgrades the current lock to a write lock with an infinite timeout.
    /// </summary>
    /// <returns>An <see cref="IDisposable"/> that releases the write lock when disposed.</returns>
    public IDisposable UseWriteLock()
    {
        return UseWriteLock(Timeout.Infinite);
    }

    /// <summary>
    ///     Upgrades the current lock to a write lock with the specified timeout.
    /// </summary>
    /// <param name="timeout">The timeout in milliseconds for acquiring the write lock.</param>
    /// <returns>An <see cref="IDisposable"/> that releases the write lock when disposed.</returns>
    /// <exception cref="AcquireLockFailedException">The write lock could not be acquired within the specified timeout.</exception>
    public IDisposable UseWriteLock(int timeout)
    {
        if (!_lockSlim.TryEnterWriteLock(timeout))
        {
            throw new AcquireLockFailedException("Acquire upgraded writer lock failed", timeout);
        }

        return new DelegateDisposable(() => _lockSlim.ExitWriteLock(), true);
    }

    /// <inheritdoc />
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
