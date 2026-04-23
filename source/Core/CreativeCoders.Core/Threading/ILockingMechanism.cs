using System;

namespace CreativeCoders.Core.Threading;

/// <summary>
///     Defines a mechanism for executing actions and functions within read and write locks.
/// </summary>
public interface ILockingMechanism
{
    /// <summary>
    ///     Executes the specified action within a read lock.
    /// </summary>
    /// <param name="action">The action to execute under the read lock.</param>
    void Read(Action action);

    /// <summary>
    ///     Executes the specified function within a read lock and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="function">The function to execute under the read lock.</param>
    /// <returns>The result of the function.</returns>
    T Read<T>(Func<T> function);

    /// <summary>
    ///     Executes the specified action within a write lock.
    /// </summary>
    /// <param name="action">The action to execute under the write lock.</param>
    void Write(Action action);

    /// <summary>
    ///     Executes the specified function within a write lock and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="function">The function to execute under the write lock.</param>
    /// <returns>The result of the function.</returns>
    T Write<T>(Func<T> function);
}
