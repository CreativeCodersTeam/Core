using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Comparing;

/// <summary>
/// Implements <see cref="IEqualityComparer{T}"/> using delegate functions for equality comparison and hash code generation.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
[PublicAPI]
public class DelegateEqualityComparer<T> : IEqualityComparer<T>
{
    private readonly Func<T, T, bool> _compare;

    private readonly Func<T, int> _getHashCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateEqualityComparer{T}"/> class with a comparison delegate.
    /// The default <see cref="object.GetHashCode"/> is used for hash code generation.
    /// </summary>
    /// <param name="compare">Delegate that determines whether two objects are equal.</param>
    public DelegateEqualityComparer(Func<T, T, bool> compare) : this(compare, null) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateEqualityComparer{T}"/> class with a comparison delegate
    /// and a hash code delegate.
    /// </summary>
    /// <param name="compare">Delegate that determines whether two objects are equal.</param>
    /// <param name="getHashCode">Delegate that computes the hash code for an object, or <see langword="null"/> to use the default.</param>
    public DelegateEqualityComparer(Func<T, T, bool> compare, Func<T, int> getHashCode)
    {
        Ensure.IsNotNull(compare);

        _compare = compare;
        _getHashCode = getHashCode;
    }

    /// <inheritdoc/>
    public bool Equals(T x, T y)
    {
        return _compare(x, y);
    }

    /// <inheritdoc/>
    public int GetHashCode(T obj)
    {
        return _getHashCode?.Invoke(obj) ?? obj.GetHashCode();
    }
}
