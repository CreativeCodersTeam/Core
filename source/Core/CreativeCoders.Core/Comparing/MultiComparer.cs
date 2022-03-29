using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Comparing;

[PublicAPI]
public class MultiComparer<T> : IComparer<T>
{
    private readonly IComparer<T>[] _comparerList;

    public MultiComparer(params IComparer<T>[] comparerList)
    {
        _comparerList = comparerList;
    }

    public int Compare(T x, T y)
    {
        return _comparerList
            .Select(comparer => comparer.Compare(x, y))
            .FirstOrDefault(compareResult => compareResult != 0);
    }
}