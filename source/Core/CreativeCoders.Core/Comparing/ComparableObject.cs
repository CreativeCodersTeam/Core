using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable enable
namespace CreativeCoders.Core.Comparing;

public class ComparableObject<T> : IEquatable<T>, IComparable<T>
    where T : ComparableObject<T>
{
    private static IEqualityComparer<T> __equalityComparer = EqualityComparer<T>.Default;

    private static IComparer<T> __comparer =
        new FuncComparer<T, string?>(x => x?.ToString(), SortOrder.Ascending);

    private static Func<T, int> __getHashCodeFunc = RuntimeHelpers.GetHashCode;

    protected static void InitComparableObject<TProperty>(Func<T, TProperty> getCompareProperty)
    {
        __equalityComparer = new FuncEqualityComparer<T, TProperty>(getCompareProperty);

        __comparer = new FuncComparer<T, TProperty>(getCompareProperty, SortOrder.Ascending);

        __getHashCodeFunc = x => __equalityComparer.GetHashCode(x);
    }

    public override bool Equals(object? obj) => Equals(obj as T);

    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return __getHashCodeFunc((T)this);
    }

    public int CompareTo(T? other)
    {
        return __comparer.Compare((T)this, other);
    }

    public bool Equals(T? other)
    {
        return __equalityComparer.Equals((T)this, other);
    }
}

public class ComparableObject<TObject, TInterface> : IEquatable<TInterface>, IComparable<TInterface>
    where TObject : ComparableObject<TObject, TInterface>, TInterface
    where TInterface : class
{
    private static IEqualityComparer<TInterface> __equalityComparer = EqualityComparer<TInterface>.Default;

    private static IComparer<TInterface> __comparer =
        new FuncComparer<TInterface, string?>(x => x?.ToString(), SortOrder.Ascending);

    private static Func<TInterface, int> __getHashCodeFunc = RuntimeHelpers.GetHashCode;

    protected static void InitComparableObject<TProperty>(Func<TInterface, TProperty> getCompareProperty)
    {
        __equalityComparer = new FuncEqualityComparer<TInterface, TProperty>(getCompareProperty);

        __comparer = new FuncComparer<TInterface, TProperty>(getCompareProperty, SortOrder.Ascending);

        __getHashCodeFunc = x => __equalityComparer.GetHashCode(x);
    }

    public override bool Equals(object? obj) => Equals(obj as TInterface);

    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return __getHashCodeFunc((TInterface)(object)this);
    }

    public int CompareTo(TInterface? other)
    {
        return __comparer.Compare((TInterface)(object)this, other);
    }

    public bool Equals(TInterface? other)
    {
        return __equalityComparer.Equals((TInterface)(object)this, other);
    }
}
