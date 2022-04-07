using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Comparing;

[PublicAPI]
public class DelegateEqualityComparer<T> : IEqualityComparer<T>
{
    private readonly Func<T, T, bool> _compare;

    private readonly Func<T, int> _getHashCode;

    public DelegateEqualityComparer(Func<T, T, bool> compare) : this(compare, null) { }

    public DelegateEqualityComparer(Func<T, T, bool> compare, Func<T, int> getHashCode)
    {
        Ensure.IsNotNull(compare, nameof(compare));

        _compare = compare;
        _getHashCode = getHashCode;
    }

    public bool Equals(T x, T y)
    {
        return _compare(x, y);
    }

    public int GetHashCode(T obj)
    {
        return _getHashCode?.Invoke(obj) ?? obj.GetHashCode();
    }
}
