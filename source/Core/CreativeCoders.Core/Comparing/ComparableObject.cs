using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#nullable enable
namespace CreativeCoders.Core.Comparing;

/// <summary>
/// Provides a base class for objects that support equality and comparison based on a configurable property.
/// </summary>
/// <typeparam name="T">The derived type that extends this base class.</typeparam>
public class ComparableObject<T> : IEquatable<T>, IComparable<T>
    where T : ComparableObject<T>
{
    private static IEqualityComparer<T> __equalityComparer = EqualityComparer<T>.Default;

    private static IComparer<T> __comparer =
        new FuncComparer<T, string?>(x => x?.ToString(), SortOrder.Ascending);

    private static Func<T, int> __getHashCodeFunc = RuntimeHelpers.GetHashCode;

    /// <summary>
    /// Initializes the equality comparer, comparer, and hash code function based on the specified property selector.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property used for comparison.</typeparam>
    /// <param name="getCompareProperty">Function that selects the property to use for comparison and equality.</param>
    protected static void InitComparableObject<TProperty>(Func<T, TProperty> getCompareProperty)
    {
        __equalityComparer = new FuncEqualityComparer<T, TProperty>(getCompareProperty);

        __comparer = new FuncComparer<T, TProperty>(getCompareProperty, SortOrder.Ascending);

        __getHashCodeFunc = x => __equalityComparer.GetHashCode(x);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as T);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return __getHashCodeFunc((T)this);
    }

    /// <inheritdoc/>
    public int CompareTo(T? other)
    {
        return __comparer.Compare((T)this, other);
    }

    /// <inheritdoc/>
    public bool Equals(T? other)
    {
        return __equalityComparer.Equals((T)this, other);
    }
}

/// <summary>
/// Provides a base class for objects that support equality and comparison through an interface type,
/// based on a configurable property.
/// </summary>
/// <typeparam name="TObject">The concrete derived type that extends this base class and implements <typeparamref name="TInterface"/>.</typeparam>
/// <typeparam name="TInterface">The interface type used for equality and comparison operations.</typeparam>
public class ComparableObject<TObject, TInterface> : IEquatable<TInterface>, IComparable<TInterface>
    where TObject : ComparableObject<TObject, TInterface>, TInterface
    where TInterface : class
{
    private static IEqualityComparer<TInterface> __equalityComparer = EqualityComparer<TInterface>.Default;

    private static IComparer<TInterface> __comparer =
        new FuncComparer<TInterface, string?>(x => x?.ToString(), SortOrder.Ascending);

    private static Func<TInterface, int> __getHashCodeFunc = RuntimeHelpers.GetHashCode;

    /// <summary>
    /// Initializes the equality comparer, comparer, and hash code function based on the specified property selector.
    /// </summary>
    /// <typeparam name="TProperty">The type of the property used for comparison.</typeparam>
    /// <param name="getCompareProperty">Function that selects the property to use for comparison and equality.</param>
    protected static void InitComparableObject<TProperty>(Func<TInterface, TProperty> getCompareProperty)
    {
        __equalityComparer = new FuncEqualityComparer<TInterface, TProperty>(getCompareProperty);

        __comparer = new FuncComparer<TInterface, TProperty>(getCompareProperty, SortOrder.Ascending);

        __getHashCodeFunc = x => __equalityComparer.GetHashCode(x);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as TInterface);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return __getHashCodeFunc((TInterface)(object)this);
    }

    /// <inheritdoc/>
    public int CompareTo(TInterface? other)
    {
        return __comparer.Compare((TInterface)(object)this, other);
    }

    /// <inheritdoc/>
    public bool Equals(TInterface? other)
    {
        return __equalityComparer.Equals((TInterface)(object)this, other);
    }
}
