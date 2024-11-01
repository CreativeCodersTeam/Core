using System;
using System.Threading;

namespace CreativeCoders.Core.Threading;

public class AcquireLockFailedException : ApplicationException
{
    public AcquireLockFailedException(string message, int timeout) : base(message)
    {
        Timeout = timeout;
    }

    public AcquireLockFailedException(string message, int timeout,
        LockRecursionException lockRecursionException)
        : base(message, lockRecursionException)
    {
        Timeout = timeout;
    }

    public int Timeout { get; }
}
