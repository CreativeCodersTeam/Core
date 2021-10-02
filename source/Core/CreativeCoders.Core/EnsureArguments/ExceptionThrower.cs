using System;
using System.Diagnostics.CodeAnalysis;

#nullable enable
// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core
{
    public static class ExceptionThrower
    {
        [DoesNotReturn]
        public static void ThrowArgumentNullException(string paramName, string? message = null)
        {
            throw message == null
                ? new ArgumentNullException(paramName)
                : new ArgumentNullException(paramName, message);
        }
    }
}
