using System;
using System.Threading;

namespace CreativeCoders.Core.Threading;

/// <summary>
///     Provides extension methods for executing actions and functions within
///     <see cref="ReaderWriterLockSlim"/> read and write locks, and within
///     <see langword="lock"/> statements on arbitrary objects.
/// </summary>
public static class LockExtension
{
    /// <summary>
    ///     Executes the specified action within a reader lock.
    /// </summary>
    /// <param name="self">The reader-writer lock.</param>
    /// <param name="action">The action to execute under the read lock.</param>
    public static void Read(this ReaderWriterLockSlim self, Action action)
    {
        using (new AcquireReaderLock(self))
        {
            action();
        }
    }

    /// <summary>
    ///     Executes the specified action within a reader lock with a timeout.
    /// </summary>
    /// <param name="self">The reader-writer lock.</param>
    /// <param name="action">The action to execute under the read lock.</param>
    /// <param name="timeout">The timeout in milliseconds for acquiring the lock.</param>
    /// <exception cref="AcquireLockFailedException">The lock could not be acquired within the specified timeout.</exception>
    public static void Read(this ReaderWriterLockSlim self, Action action, int timeout)
    {
        using (new AcquireReaderLock(self, timeout))
        {
            action();
        }
    }

    /// <summary>
    ///     Executes the specified function within a reader lock and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="self">The reader-writer lock.</param>
    /// <param name="function">The function to execute under the read lock.</param>
    /// <returns>The result of the function.</returns>
    public static T Read<T>(this ReaderWriterLockSlim self, Func<T> function)
    {
        using (new AcquireReaderLock(self))
        {
            return function();
        }
    }

    /// <summary>
    ///     Executes the specified function within a reader lock with a timeout and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="self">The reader-writer lock.</param>
    /// <param name="function">The function to execute under the read lock.</param>
    /// <param name="timeout">The timeout in milliseconds for acquiring the lock.</param>
    /// <returns>The result of the function.</returns>
    /// <exception cref="AcquireLockFailedException">The lock could not be acquired within the specified timeout.</exception>
    public static T Read<T>(this ReaderWriterLockSlim self, Func<T> function, int timeout)
    {
        using (new AcquireReaderLock(self, timeout))
        {
            return function();
        }
    }

    /// <summary>
    ///     Executes the specified action within a writer lock.
    /// </summary>
    /// <param name="self">The reader-writer lock.</param>
    /// <param name="action">The action to execute under the write lock.</param>
    public static void Write(this ReaderWriterLockSlim self, Action action)
    {
        using (new AcquireWriterLock(self))
        {
            action();
        }
    }

    /// <summary>
    ///     Executes the specified action within a writer lock with a timeout.
    /// </summary>
    /// <param name="self">The reader-writer lock.</param>
    /// <param name="action">The action to execute under the write lock.</param>
    /// <param name="timeout">The timeout in milliseconds for acquiring the lock.</param>
    /// <exception cref="AcquireLockFailedException">The lock could not be acquired within the specified timeout.</exception>
    public static void Write(this ReaderWriterLockSlim self, Action action, int timeout)
    {
        using (new AcquireWriterLock(self, timeout))
        {
            action();
        }
    }

    /// <summary>
    ///     Executes the specified function within a writer lock and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="self">The reader-writer lock.</param>
    /// <param name="function">The function to execute under the write lock.</param>
    /// <returns>The result of the function.</returns>
    public static T Write<T>(this ReaderWriterLockSlim self, Func<T> function)
    {
        using (new AcquireWriterLock(self))
        {
            return function();
        }
    }

    /// <summary>
    ///     Executes the specified function within a writer lock with a timeout and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="self">The reader-writer lock.</param>
    /// <param name="function">The function to execute under the write lock.</param>
    /// <param name="timeout">The timeout in milliseconds for acquiring the lock.</param>
    /// <returns>The result of the function.</returns>
    /// <exception cref="AcquireLockFailedException">The lock could not be acquired within the specified timeout.</exception>
    public static T Write<T>(this ReaderWriterLockSlim self, Func<T> function, int timeout)
    {
        using (new AcquireWriterLock(self, timeout))
        {
            return function();
        }
    }

    /// <summary>
    ///     Executes the specified action within a <see langword="lock"/> statement on the object.
    /// </summary>
    /// <param name="self">The object to lock on.</param>
    /// <param name="action">The action to execute under the lock.</param>
    public static void Lock(this object self, Action action)
    {
        lock (self)
        {
            action();
        }
    }

    /// <summary>
    ///     Executes the specified function within a <see langword="lock"/> statement on the object
    ///     and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="self">The object to lock on.</param>
    /// <param name="function">The function to execute under the lock.</param>
    /// <returns>The result of the function.</returns>
    public static T Lock<T>(this object self, Func<T> function)
    {
        lock (self)
        {
            return function();
        }
    }
}
