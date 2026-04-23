using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Comparing;

/// <summary>
/// Implements <see cref="IEqualityComparer{T}"/> by combining multiple equality comparers.
/// Two objects are considered equal only if all comparers agree they are equal.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
[PublicAPI]
public class MultiEqualityComparer<T> : IEqualityComparer<T>
{
    private readonly IEqualityComparer<T>[] _comparerList;

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiEqualityComparer{T}"/> class.
    /// </summary>
    /// <param name="comparerList">Ordered collection of equality comparers that must all agree for two objects to be considered equal.</param>
    public MultiEqualityComparer(params IEqualityComparer<T>[] comparerList)
    {
        Ensure.IsNotNullOrEmpty(comparerList);

        _comparerList = comparerList;
    }

    /// <inheritdoc/>
    public bool Equals(T x, T y)
    {
        return _comparerList.All(comparer => comparer.Equals(x, y));
    }

    /// <inheritdoc/>
    public int GetHashCode(T obj)
    {
        if (_comparerList.Length == 1)
        {
            return _comparerList[0].GetHashCode(obj);
        }

        var hashCodes = _comparerList.Select(comparer => comparer.GetHashCode(obj));
        return string.Concat(hashCodes).GetHashCode();
    }
}
