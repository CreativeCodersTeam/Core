using System.Threading.Tasks;
using JetBrains.Annotations;

namespace CreativeCoders.Core.Caching;

/// <summary>
/// Represents a lazily resolved cached value that retrieves its data from an underlying cache.
/// </summary>
/// <typeparam name="TValue">The type of the cached value.</typeparam>
[PublicAPI]
public interface ICachedValue<TValue>
{
    /// <summary>
    /// Asynchronously retrieves the cached value.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the cached value.</returns>
    Task<TValue> GetValueAsync();

    /// <summary>
    /// Gets the cached value synchronously.
    /// </summary>
    TValue Value { get; }
}
