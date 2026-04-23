#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace CreativeCoders.Core;

/// <summary>
/// Provides a singleton no-op <see cref="Action"/> delegate.
/// </summary>
[ExcludeFromCodeCoverage]
[PublicAPI]
public static class NullAction
{
    /// <summary>
    /// Gets a no-op <see cref="Action"/> instance.
    /// </summary>
    public static Action Instance { get; } = () => { };
}

/// <summary>
/// Provides a singleton no-op <see cref="Action{T}"/> delegate.
/// </summary>
/// <typeparam name="T">The type of the action parameter.</typeparam>
[ExcludeFromCodeCoverage]
[PublicAPI]
public static class NullAction<T>
{
    /// <summary>
    /// Gets a no-op <see cref="Action{T}"/> instance.
    /// </summary>
    public static Action<T> Instance { get; } = _ => { };
}
