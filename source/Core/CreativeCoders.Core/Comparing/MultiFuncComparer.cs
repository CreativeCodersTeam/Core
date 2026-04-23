namespace CreativeCoders.Core.Comparing;
//public class MultiFuncComparer<T, TKey> : MultiComparer<T>
//{
//    public MultiFuncComparer(SortFieldInfo<T, TKey> sortFieldInfo) :
//        base(new FuncComparer<T, TKey>(sortFieldInfo.KeySelector, sortFieldInfo.SortOrder))
//    {
//    }
//}

/// <summary>
/// Implements <see cref="MultiComparer{T}"/> using two key selector functions defined by <see cref="SortFieldInfo{T, TKey}"/> instances.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
/// <typeparam name="TKey1">The type of the first sort key.</typeparam>
/// <typeparam name="TKey2">The type of the second sort key.</typeparam>
public class MultiFuncComparer<T, TKey1, TKey2> : MultiComparer<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFuncComparer{T, TKey1, TKey2}"/> class.
    /// </summary>
    /// <param name="sortFieldInfo1">Sort field definition for the first key.</param>
    /// <param name="sortFieldInfo2">Sort field definition for the second key.</param>
    public MultiFuncComparer(SortFieldInfo<T, TKey1> sortFieldInfo1, SortFieldInfo<T, TKey2> sortFieldInfo2) :
        base(new FuncComparer<T, TKey1>(sortFieldInfo1.KeySelector, sortFieldInfo1.SortOrder),
            new FuncComparer<T, TKey2>(sortFieldInfo2.KeySelector, sortFieldInfo2.SortOrder)) { }
}

/// <summary>
/// Implements <see cref="MultiComparer{T}"/> using three key selector functions defined by <see cref="SortFieldInfo{T, TKey}"/> instances.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
/// <typeparam name="TKey1">The type of the first sort key.</typeparam>
/// <typeparam name="TKey2">The type of the second sort key.</typeparam>
/// <typeparam name="TKey3">The type of the third sort key.</typeparam>
public class MultiFuncComparer<T, TKey1, TKey2, TKey3> : MultiComparer<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFuncComparer{T, TKey1, TKey2, TKey3}"/> class.
    /// </summary>
    /// <param name="sortFieldInfo1">Sort field definition for the first key.</param>
    /// <param name="sortFieldInfo2">Sort field definition for the second key.</param>
    /// <param name="sortFieldInfo3">Sort field definition for the third key.</param>
    public MultiFuncComparer(SortFieldInfo<T, TKey1> sortFieldInfo1, SortFieldInfo<T, TKey2> sortFieldInfo2,
        SortFieldInfo<T, TKey3> sortFieldInfo3) :
        base(new FuncComparer<T, TKey1>(sortFieldInfo1.KeySelector, sortFieldInfo1.SortOrder),
            new FuncComparer<T, TKey2>(sortFieldInfo2.KeySelector, sortFieldInfo2.SortOrder),
            new FuncComparer<T, TKey3>(sortFieldInfo3.KeySelector, sortFieldInfo3.SortOrder)) { }
}

/// <summary>
/// Implements <see cref="MultiComparer{T}"/> using four key selector functions defined by <see cref="SortFieldInfo{T, TKey}"/> instances.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
/// <typeparam name="TKey1">The type of the first sort key.</typeparam>
/// <typeparam name="TKey2">The type of the second sort key.</typeparam>
/// <typeparam name="TKey3">The type of the third sort key.</typeparam>
/// <typeparam name="TKey4">The type of the fourth sort key.</typeparam>
public class MultiFuncComparer<T, TKey1, TKey2, TKey3, TKey4> : MultiComparer<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFuncComparer{T, TKey1, TKey2, TKey3, TKey4}"/> class.
    /// </summary>
    /// <param name="sortFieldInfo1">Sort field definition for the first key.</param>
    /// <param name="sortFieldInfo2">Sort field definition for the second key.</param>
    /// <param name="sortFieldInfo3">Sort field definition for the third key.</param>
    /// <param name="sortFieldInfo4">Sort field definition for the fourth key.</param>
    public MultiFuncComparer(SortFieldInfo<T, TKey1> sortFieldInfo1, SortFieldInfo<T, TKey2> sortFieldInfo2,
        SortFieldInfo<T, TKey3> sortFieldInfo3, SortFieldInfo<T, TKey4> sortFieldInfo4) :
        base(new FuncComparer<T, TKey1>(sortFieldInfo1.KeySelector, sortFieldInfo1.SortOrder),
            new FuncComparer<T, TKey2>(sortFieldInfo2.KeySelector, sortFieldInfo2.SortOrder),
            new FuncComparer<T, TKey3>(sortFieldInfo3.KeySelector, sortFieldInfo3.SortOrder),
            new FuncComparer<T, TKey4>(sortFieldInfo4.KeySelector, sortFieldInfo4.SortOrder)) { }
}

/// <summary>
/// Implements <see cref="MultiComparer{T}"/> using five key selector functions defined by <see cref="SortFieldInfo{T, TKey}"/> instances.
/// </summary>
/// <typeparam name="T">The type of objects to compare.</typeparam>
/// <typeparam name="TKey1">The type of the first sort key.</typeparam>
/// <typeparam name="TKey2">The type of the second sort key.</typeparam>
/// <typeparam name="TKey3">The type of the third sort key.</typeparam>
/// <typeparam name="TKey4">The type of the fourth sort key.</typeparam>
/// <typeparam name="TKey5">The type of the fifth sort key.</typeparam>
public class MultiFuncComparer<T, TKey1, TKey2, TKey3, TKey4, TKey5> : MultiComparer<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MultiFuncComparer{T, TKey1, TKey2, TKey3, TKey4, TKey5}"/> class.
    /// </summary>
    /// <param name="sortFieldInfo1">Sort field definition for the first key.</param>
    /// <param name="sortFieldInfo2">Sort field definition for the second key.</param>
    /// <param name="sortFieldInfo3">Sort field definition for the third key.</param>
    /// <param name="sortFieldInfo4">Sort field definition for the fourth key.</param>
    /// <param name="sortFieldInfo5">Sort field definition for the fifth key.</param>
    public MultiFuncComparer(SortFieldInfo<T, TKey1> sortFieldInfo1, SortFieldInfo<T, TKey2> sortFieldInfo2,
        SortFieldInfo<T, TKey3> sortFieldInfo3, SortFieldInfo<T, TKey4> sortFieldInfo4,
        SortFieldInfo<T, TKey5> sortFieldInfo5) :
        base(new FuncComparer<T, TKey1>(sortFieldInfo1.KeySelector, sortFieldInfo1.SortOrder),
            new FuncComparer<T, TKey2>(sortFieldInfo2.KeySelector, sortFieldInfo2.SortOrder),
            new FuncComparer<T, TKey3>(sortFieldInfo3.KeySelector, sortFieldInfo3.SortOrder),
            new FuncComparer<T, TKey4>(sortFieldInfo4.KeySelector, sortFieldInfo4.SortOrder),
            new FuncComparer<T, TKey5>(sortFieldInfo5.KeySelector, sortFieldInfo5.SortOrder)) { }
}
