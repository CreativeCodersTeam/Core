using System;
using System.Threading;

namespace CreativeCoders.Core.Threading;

public class MutexLock : IDisposable
{
    private Mutex _mutex;

    private bool _disposed;

    private readonly bool _hasMutexLock;

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

    public void Dispose()
    {
        Dispose(true);
    }
}