using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

#nullable enable
// ReSharper disable once CheckNamespace
namespace CreativeCoders.Core;

/// <summary>Provides methods for throwing standard argument exceptions.</summary>
public static class ExceptionThrower
{
    /// <summary>Throws an <see cref="ArgumentNullException"/> with the specified parameter name.</summary>
    /// <param name="paramName">The name of the parameter that caused the exception.</param>
    /// <param name="message">The optional error message.</param>
    /// <exception cref="ArgumentNullException"><paramref name="paramName"/> is always thrown as this method never returns normally.</exception>
    [DoesNotReturn]
    [ContractAnnotation("=> halt")]
    public static void ThrowArgumentNullException(string paramName, string? message = null)
    {
        throw message == null
            ? new ArgumentNullException(paramName)
            : new ArgumentNullException(paramName, message);
    }
}
