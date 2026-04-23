using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Comparing;

/// <summary>
/// Implements <see cref="IComparer{T}"/> by combining multiple comparers, evaluating them in order
/// and returning the first non-zero result.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
[PublicAPI]
public class MultiComparer<T> : IComparer<T>
{
    private readonly IComparer<T>[] _comparerList;

    /// <summary>
    /// Initializes a new instance of the <see cref="MultiComparer{T}"/> class.
    /// </summary>
    /// <param name="comparerList">Ordered collection of comparers to evaluate sequentially.</param>
    public MultiComparer(params IComparer<T>[] comparerList)
    {
        _comparerList = comparerList;
    }

    /// <inheritdoc/>
    public int Compare(T x, T y)
    {
        return _comparerList
            .Select(comparer => comparer.Compare(x, y))
            .FirstOrDefault(compareResult => compareResult != 0);
    }
}
