using System;

#nullable enable
// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core
{
    public static class MustArgumentExtensions
    {
        public static ref readonly Argument<T> Must<T>(in this Argument<T> argument, Func<T?, bool> predicate,
            string? message = null)
        {
            if (!predicate(argument.Value))
            {
                throw new ArgumentException(message ?? $"Argument must '{predicate}'", argument.Name);
            }

            return ref argument;
        }

        public static ref readonly Argument<T> MustNot<T>(in this Argument<T> argument, Func<T?, bool> predicate,
            string? message = null)
        {
            if (predicate(argument.Value))
            {
                throw new ArgumentException(message ?? $"Argument must not '{predicate}'", argument.Name);
            }

            return ref argument;
        }
        
        public static ref readonly ArgumentNotNull<T> Must<T>(in this ArgumentNotNull<T> argument, Func<T, bool> predicate,
            string? message = null)
        {
            if (!predicate(argument.Value))
            {
                throw new ArgumentException(message ?? $"Argument must '{predicate}'", argument.Name);
            }

            return ref argument;
        }

        public static ref readonly ArgumentNotNull<T> MustNot<T>(in this ArgumentNotNull<T> argument, Func<T, bool> predicate,
            string? message = null)
        {
            if (predicate(argument.Value))
            {
                throw new ArgumentException(message ?? $"Argument must not '{predicate}'", argument.Name);
            }

            return ref argument;
        }
    }
}
