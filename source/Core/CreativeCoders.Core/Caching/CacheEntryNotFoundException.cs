using System;

namespace CreativeCoders.Core.Caching
{
    [Serializable]
    public class CacheEntryNotFoundException : Exception
    {
        public CacheEntryNotFoundException(string key) : base($"No cache entry for key = '{key}' found")
        {
            Key = key;
        }        

        public string Key { get; }
    }
}