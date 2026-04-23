#nullable enable

namespace CreativeCoders.Core.Collections;

/// <summary>
///     Pairs an element of type <typeparamref name="T"/> with its zero-based index
///     within a sequence.
/// </summary>
/// <typeparam name="T">The type of the element.</typeparam>
public class ItemWithIndex<T>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ItemWithIndex{T}"/> class.
    /// </summary>
    /// <param name="index">The zero-based position of the element in the source sequence.</param>
    /// <param name="data">The element value.</param>
    public ItemWithIndex(int index, T data)
    {
        Index = index;
        Data = data;
    }

    /// <summary>
    ///     Gets the zero-based position of the element in the source sequence.
    /// </summary>
    public int Index { get; }

    /// <summary>
    ///     Gets the element value.
    /// </summary>
    public T Data { get; }
}
