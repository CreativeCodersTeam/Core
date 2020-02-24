using System;

namespace CreativeCoders.Core.Weak
{
    public static class WeakReferenceExtensions
    {
        public static T GetTarget<T>(this WeakReference<T> weakReference)
            where T : class
        {
            return weakReference.TryGetTarget(out var target)
                ? target
                : default;
        }

        public static bool GetIsAlive<T>(this WeakReference<T> weakReference)
            where T : class
        {
            return weakReference.TryGetTarget(out _);
        }
    }
}