using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Error;

/// <summary>
/// Implements <see cref="IErrorHandler"/> as a no-op that silently ignores all exceptions.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public class NullErrorHandler : IErrorHandler
{
    /// <summary>
    /// Gets a shared instance of <see cref="NullErrorHandler"/>.
    /// </summary>
    public static IErrorHandler Instance { get; } = new NullErrorHandler();

    /// <inheritdoc />
    public void HandleException(Exception exception) { }
}
