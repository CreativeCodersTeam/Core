namespace CreativeCoders.Core.Caching;

public class CacheRequestResult<TValue>
{
    public CacheRequestResult(bool entryExists, TValue value)
    {
        EntryExists = entryExists;
        Value = value;
    }

    public bool EntryExists { get; }

    public TValue Value { get; }
}