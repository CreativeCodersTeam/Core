using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

#nullable enable

namespace CreativeCoders.Core;

/// <summary>
/// Provides a singleton null object instance.
/// </summary>
[PublicAPI]
[ExcludeFromCodeCoverage]
public static class NullObject
{
    /// <summary>
    /// Gets a shared <see cref="object"/> instance.
    /// </summary>
    public static object Instance { get; } = new object();
}
