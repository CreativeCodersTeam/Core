using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Weak;

/// <summary>
/// Represents a no-op <see cref="IDisposable"/> implementation that performs no action when disposed.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public sealed class NullDisposable : IDisposable
{
    /// <inheritdoc/>
    public void Dispose() { }
}
