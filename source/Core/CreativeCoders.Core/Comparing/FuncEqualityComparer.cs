using System;
using System.Collections.Generic;

namespace CreativeCoders.Core.Comparing;

public class FuncEqualityComparer<T, TKey> : IEqualityComparer<T>
{
    private readonly Func<T, TKey> _keySelector;

    public FuncEqualityComparer(Func<T, TKey> keySelector)
    {
        Ensure.IsNotNull(keySelector, nameof(keySelector));

        _keySelector = keySelector;
    }

    public bool Equals(T x, T y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x is null || y is null)
        {
            return false;
        }

        var xValue = _keySelector(x);
        var yValue = _keySelector(y);

        return EqualityComparer<TKey>.Default.Equals(xValue, yValue);
    }

    public int GetHashCode(T obj)
    {
        return _keySelector(obj)?.GetHashCode() ?? 0;
    }
}