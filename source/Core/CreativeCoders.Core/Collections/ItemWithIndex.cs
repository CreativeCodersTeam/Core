namespace CreativeCoders.Core.Collections;

public class ItemWithIndex<T>
{
    public ItemWithIndex(int index, T data)
    {
        Index = index;
        Data = data;
    }

    public int Index { get; }

    public T Data { get; }
}