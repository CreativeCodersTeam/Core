using System;

namespace CreativeCoders.Core.Weak;

/// <summary>
/// Provides extension methods for <see cref="WeakReference{T}"/>.
/// </summary>
public static class WeakReferenceExtensions
{
    /// <summary>
    /// Retrieves the target object from the weak reference.
    /// </summary>
    /// <typeparam name="T">The type of the referenced object.</typeparam>
    /// <param name="weakReference">The weak reference to retrieve the target from.</param>
    /// <returns>The target object if it is still alive; otherwise, <see langword="null"/>.</returns>
    public static T GetTarget<T>(this WeakReference<T> weakReference)
        where T : class
    {
        return weakReference.TryGetTarget(out var target)
            ? target
            : null;
    }

    /// <summary>
    /// Determines whether the target of the weak reference is still alive.
    /// </summary>
    /// <typeparam name="T">The type of the referenced object.</typeparam>
    /// <param name="weakReference">The weak reference to check.</param>
    /// <returns><see langword="true"/> if the target is still alive; otherwise, <see langword="false"/>.</returns>
    public static bool GetIsAlive<T>(this WeakReference<T> weakReference)
        where T : class
    {
        return weakReference.TryGetTarget(out _);
    }
}
