using System;
using System.Collections.Generic;
using System.Linq;

namespace CreativeCoders.Core.Comparing;

/// <summary>
/// Implements <see cref="MultiEqualityComparer{T}"/> using multiple key selector functions of the same key type.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
/// <typeparam name="TKey">The type of the key used for equality comparison.</typeparam>
public class MultiFuncEqualityComparer<T, TKey> : MultiEqualityComparer<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFuncEqualityComparer{T, TKey}"/> class.
    /// </summary>
    /// <param name="keySelectors">Collection of functions that extract equality keys from an object.</param>
    public MultiFuncEqualityComparer(params Func<T, TKey>[] keySelectors)
        : base(keySelectors.Select(IEqualityComparer<T> (func) => new FuncEqualityComparer<T, TKey>(func))
            .ToArray()) { }
}

/// <summary>
/// Implements <see cref="MultiEqualityComparer{T}"/> using two key selector functions with distinct key types.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
/// <typeparam name="TKey1">The type of the first equality key.</typeparam>
/// <typeparam name="TKey2">The type of the second equality key.</typeparam>
public class MultiFuncEqualityComparer<T, TKey1, TKey2> : MultiEqualityComparer<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFuncEqualityComparer{T, TKey1, TKey2}"/> class.
    /// </summary>
    /// <param name="keySelector1">Function that extracts the first equality key from an object.</param>
    /// <param name="keySelector2">Function that extracts the second equality key from an object.</param>
    public MultiFuncEqualityComparer(Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2) :
        base(new FuncEqualityComparer<T, TKey1>(keySelector1),
            new FuncEqualityComparer<T, TKey2>(keySelector2)) { }
}

/// <summary>
/// Implements <see cref="MultiEqualityComparer{T}"/> using three key selector functions with distinct key types.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
/// <typeparam name="TKey1">The type of the first equality key.</typeparam>
/// <typeparam name="TKey2">The type of the second equality key.</typeparam>
/// <typeparam name="TKey3">The type of the third equality key.</typeparam>
public class MultiFuncEqualityComparer<T, TKey1, TKey2, TKey3> : MultiEqualityComparer<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFuncEqualityComparer{T, TKey1, TKey2, TKey3}"/> class.
    /// </summary>
    /// <param name="keySelector1">Function that extracts the first equality key from an object.</param>
    /// <param name="keySelector2">Function that extracts the second equality key from an object.</param>
    /// <param name="keySelector3">Function that extracts the third equality key from an object.</param>
    public MultiFuncEqualityComparer(Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2,
        Func<T, TKey3> keySelector3) :
        base(new FuncEqualityComparer<T, TKey1>(keySelector1),
            new FuncEqualityComparer<T, TKey2>(keySelector2),
            new FuncEqualityComparer<T, TKey3>(keySelector3)) { }
}

/// <summary>
/// Implements <see cref="MultiEqualityComparer{T}"/> using four key selector functions with distinct key types.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
/// <typeparam name="TKey1">The type of the first equality key.</typeparam>
/// <typeparam name="TKey2">The type of the second equality key.</typeparam>
/// <typeparam name="TKey3">The type of the third equality key.</typeparam>
/// <typeparam name="TKey4">The type of the fourth equality key.</typeparam>
public class MultiFuncEqualityComparer<T, TKey1, TKey2, TKey3, TKey4> : MultiEqualityComparer<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFuncEqualityComparer{T, TKey1, TKey2, TKey3, TKey4}"/> class.
    /// </summary>
    /// <param name="keySelector1">Function that extracts the first equality key from an object.</param>
    /// <param name="keySelector2">Function that extracts the second equality key from an object.</param>
    /// <param name="keySelector3">Function that extracts the third equality key from an object.</param>
    /// <param name="keySelector4">Function that extracts the fourth equality key from an object.</param>
    public MultiFuncEqualityComparer(Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2,
        Func<T, TKey3> keySelector3, Func<T, TKey4> keySelector4) :
        base(new FuncEqualityComparer<T, TKey1>(keySelector1),
            new FuncEqualityComparer<T, TKey2>(keySelector2),
            new FuncEqualityComparer<T, TKey3>(keySelector3),
            new FuncEqualityComparer<T, TKey4>(keySelector4)) { }
}

/// <summary>
/// Implements <see cref="MultiEqualityComparer{T}"/> using five key selector functions with distinct key types.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
/// <typeparam name="TKey1">The type of the first equality key.</typeparam>
/// <typeparam name="TKey2">The type of the second equality key.</typeparam>
/// <typeparam name="TKey3">The type of the third equality key.</typeparam>
/// <typeparam name="TKey4">The type of the fourth equality key.</typeparam>
/// <typeparam name="TKey5">The type of the fifth equality key.</typeparam>
public class MultiFuncEqualityComparer<T, TKey1, TKey2, TKey3, TKey4, TKey5> : MultiEqualityComparer<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFuncEqualityComparer{T, TKey1, TKey2, TKey3, TKey4, TKey5}"/> class.
    /// </summary>
    /// <param name="keySelector1">Function that extracts the first equality key from an object.</param>
    /// <param name="keySelector2">Function that extracts the second equality key from an object.</param>
    /// <param name="keySelector3">Function that extracts the third equality key from an object.</param>
    /// <param name="keySelector4">Function that extracts the fourth equality key from an object.</param>
    /// <param name="keySelector5">Function that extracts the fifth equality key from an object.</param>
    public MultiFuncEqualityComparer(Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2,
        Func<T, TKey3> keySelector3, Func<T, TKey4> keySelector4, Func<T, TKey5> keySelector5) :
        base(new FuncEqualityComparer<T, TKey1>(keySelector1),
            new FuncEqualityComparer<T, TKey2>(keySelector2),
            new FuncEqualityComparer<T, TKey3>(keySelector3),
            new FuncEqualityComparer<T, TKey4>(keySelector4),
            new FuncEqualityComparer<T, TKey5>(keySelector5)) { }
}
