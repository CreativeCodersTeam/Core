using System;

namespace CreativeCoders.Core.Threading;

/// <summary>
///     Represents an action that executes within an upgradeable read lock and can optionally
///     upgrade to a write lock by calling the provided factory.
/// </summary>
/// <param name="useWriteLock">
///     A factory that acquires a write lock and returns an <see cref="IDisposable"/>
///     that releases it when disposed.
/// </param>
public delegate void UpgradeableReadAction(Func<IDisposable> useWriteLock);
