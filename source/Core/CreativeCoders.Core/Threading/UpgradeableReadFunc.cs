using System;

namespace CreativeCoders.Core.Threading;

/// <summary>
///     Represents a function that executes within an upgradeable read lock and can optionally
///     upgrade to a write lock by calling the provided factory.
/// </summary>
/// <typeparam name="T">The type of the return value.</typeparam>
/// <param name="useWriteLock">
///     A factory that acquires a write lock and returns an <see cref="IDisposable"/>
///     that releases it when disposed.
/// </param>
/// <returns>The result of the function.</returns>
public delegate T UpgradeableReadFunc<out T>(Func<IDisposable> useWriteLock);
