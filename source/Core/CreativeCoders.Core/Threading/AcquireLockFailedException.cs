using System;
using System.Threading;

namespace CreativeCoders.Core.Threading;

/// <summary>
///     The exception that is thrown when a lock cannot be acquired within the specified timeout
///     or due to a lock recursion violation.
/// </summary>
public class AcquireLockFailedException : Exception
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AcquireLockFailedException"/> class
    ///     with the specified message and timeout.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="timeout">The timeout in milliseconds that was exceeded.</param>
    public AcquireLockFailedException(string message, int timeout) : base(message)
    {
        Timeout = timeout;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="AcquireLockFailedException"/> class
    ///     with the specified message, timeout, and inner <see cref="LockRecursionException"/>.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="timeout">The timeout in milliseconds that was exceeded.</param>
    /// <param name="lockRecursionException">The inner exception that caused this exception.</param>
    public AcquireLockFailedException(string message, int timeout,
        LockRecursionException lockRecursionException)
        : base(message, lockRecursionException)
    {
        Timeout = timeout;
    }

    /// <summary>
    ///     Gets the timeout in milliseconds that was exceeded when attempting to acquire the lock.
    /// </summary>
    public int Timeout { get; }
}
