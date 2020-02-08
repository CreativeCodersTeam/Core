using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching
{
    [PublicAPI]
    public interface ICacheManager
    {
        ICache<TKey, TValue> GetCache<TKey, TValue>(string name);
    }
}