using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching;

/// <summary>
/// Defines a key-value cache that supports regions, expiration policies, and asynchronous operations.
/// </summary>
/// <typeparam name="TKey">The type of the cache key.</typeparam>
/// <typeparam name="TValue">The type of the cached value.</typeparam>
[PublicAPI]
public interface ICache<in TKey, TValue>
{
    /// <summary>
    /// Gets the value associated with the specified key, or adds a new value using the provided factory if the key does not exist.
    /// </summary>
    /// <param name="key">Cache key to look up or insert.</param>
    /// <param name="getValue">Factory delegate invoked to produce the value when the key is not found.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    /// <returns>The cached or newly added value.</returns>
    TValue GetOrAdd(TKey key, Func<TValue> getValue, string regionName = null);

    /// <summary>
    /// Gets the value associated with the specified key, or adds a new value with the given expiration policy if the key does not exist.
    /// </summary>
    /// <param name="key">Cache key to look up or insert.</param>
    /// <param name="getValue">Factory delegate invoked to produce the value when the key is not found.</param>
    /// <param name="expirationPolicy">Expiration policy applied to the new cache entry.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    /// <returns>The cached or newly added value.</returns>
    TValue GetOrAdd(TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy,
        string regionName = null);

    /// <summary>
    /// Asynchronously gets the value associated with the specified key, or adds a new value using the provided factory if the key does not exist.
    /// </summary>
    /// <param name="key">Cache key to look up or insert.</param>
    /// <param name="getValue">Factory delegate invoked to produce the value when the key is not found.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the cached or newly added value.</returns>
    Task<TValue> GetOrAddAsync(TKey key, Func<TValue> getValue, string regionName = null);

    /// <summary>
    /// Asynchronously gets the value associated with the specified key, or adds a new value with the given expiration policy if the key does not exist.
    /// </summary>
    /// <param name="key">Cache key to look up or insert.</param>
    /// <param name="getValue">Factory delegate invoked to produce the value when the key is not found.</param>
    /// <param name="expirationPolicy">Expiration policy applied to the new cache entry.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the cached or newly added value.</returns>
    Task<TValue> GetOrAddAsync(TKey key, Func<TValue> getValue, ICacheExpirationPolicy expirationPolicy,
        string regionName = null);

    /// <summary>
    /// Attempts to get the value associated with the specified key.
    /// </summary>
    /// <param name="key">Cache key to look up.</param>
    /// <param name="value">When this method returns, contains the cached value if the key was found; otherwise, the default value.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    /// <returns><see langword="true"/> if the key exists in the cache; otherwise, <see langword="false"/>.</returns>
    bool TryGet(TKey key, out TValue value, string regionName = null);

    /// <summary>
    /// Asynchronously attempts to get the value associated with the specified key.
    /// </summary>
    /// <param name="key">Cache key to look up.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="CacheRequestResult{TValue}"/> indicating whether the entry exists and its value.</returns>
    Task<CacheRequestResult<TValue>> TryGetAsync(TKey key, string regionName = null);

    /// <summary>
    /// Adds a new entry or updates an existing entry in the cache with the specified key and value.
    /// </summary>
    /// <param name="key">Cache key to add or update.</param>
    /// <param name="value">Value to store in the cache.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    void AddOrUpdate(TKey key, TValue value, string regionName = null);

    /// <summary>
    /// Adds a new entry or updates an existing entry in the cache with the specified key, value, and expiration policy.
    /// </summary>
    /// <param name="key">Cache key to add or update.</param>
    /// <param name="value">Value to store in the cache.</param>
    /// <param name="expirationPolicy">Expiration policy applied to the cache entry.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    void AddOrUpdate(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy,
        string regionName = null);

    /// <summary>
    /// Asynchronously adds a new entry or updates an existing entry in the cache with the specified key and value.
    /// </summary>
    /// <param name="key">Cache key to add or update.</param>
    /// <param name="value">Value to store in the cache.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddOrUpdateAsync(TKey key, TValue value, string regionName = null);

    /// <summary>
    /// Asynchronously adds a new entry or updates an existing entry in the cache with the specified key, value, and expiration policy.
    /// </summary>
    /// <param name="key">Cache key to add or update.</param>
    /// <param name="value">Value to store in the cache.</param>
    /// <param name="expirationPolicy">Expiration policy applied to the cache entry.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task AddOrUpdateAsync(TKey key, TValue value, ICacheExpirationPolicy expirationPolicy,
        string regionName = null);

    /// <summary>
    /// Removes all entries from the cache, optionally scoped to a specific region.
    /// </summary>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is cleared.</param>
    void Clear(string regionName = null);

    /// <summary>
    /// Asynchronously removes all entries from the cache, optionally scoped to a specific region.
    /// </summary>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is cleared.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task ClearAsync(string regionName = null);

    /// <summary>
    /// Removes the entry with the specified key from the cache.
    /// </summary>
    /// <param name="key">Cache key to remove.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    void Remove(TKey key, string regionName = null);

    /// <summary>
    /// Asynchronously removes the entry with the specified key from the cache.
    /// </summary>
    /// <param name="key">Cache key to remove.</param>
    /// <param name="regionName">Optional cache region name. When <see langword="null"/>, the default region is used.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task RemoveAsync(TKey key, string regionName = null);
}
