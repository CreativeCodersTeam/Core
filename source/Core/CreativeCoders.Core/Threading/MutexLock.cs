using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace CreativeCoders.Core.Threading;

/// <summary>
///     Provides a disposable lock based on a named <see cref="Mutex"/>. Acquires the mutex
///     upon construction and releases it upon disposal.
/// </summary>
public sealed class MutexLock : IDisposable
{
    private Mutex _mutex;

    private bool _disposed;

    private readonly bool _hasMutexLock;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MutexLock"/> class and acquires
    ///     the named mutex, blocking until the mutex is available.
    /// </summary>
    /// <param name="mutexName">The name of the system mutex to acquire.</param>
    public MutexLock(string mutexName)
    {
        Ensure.IsNotNullOrWhitespace(mutexName, nameof(mutexName));

        if (!Mutex.TryOpenExisting(mutexName, out _mutex))
        {
            try
            {
                _mutex = new Mutex(false, mutexName);
            }
            catch (Exception)
            {
                _mutex = Mutex.OpenExisting(mutexName);
            }
        }

        _hasMutexLock = _mutex.WaitOne();
    }

    [ExcludeFromCodeCoverage]
    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing && _mutex != null)
        {
            if (_hasMutexLock)
            {
                _mutex.ReleaseMutex();
            }

            _mutex.Dispose();
            _mutex = null;
        }

        _disposed = true;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
    }
}
