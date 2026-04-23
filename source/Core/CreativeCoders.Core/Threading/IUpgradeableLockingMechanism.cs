using JetBrains.Annotations;

namespace CreativeCoders.Core.Threading;

/// <summary>
///     Extends <see cref="ILockingMechanism"/> with support for upgradeable read locks
///     that can be promoted to write locks when needed.
/// </summary>
[PublicAPI]
public interface IUpgradeableLockingMechanism : ILockingMechanism
{
    /// <summary>
    ///     Executes the specified action within an upgradeable read lock.
    /// </summary>
    /// <param name="action">The action to execute, which receives a factory for upgrading to a write lock.</param>
    void UpgradeableRead(UpgradeableReadAction action);

    /// <summary>
    ///     Executes the specified function within an upgradeable read lock and returns the result.
    /// </summary>
    /// <typeparam name="T">The type of the return value.</typeparam>
    /// <param name="function">The function to execute, which receives a factory for upgrading to a write lock.</param>
    /// <returns>The result of the function.</returns>
    T UpgradeableRead<T>(UpgradeableReadFunc<T> function);
}
