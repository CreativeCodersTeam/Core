using JetBrains.Annotations;

namespace CreativeCoders.Cli.Commands.Crud;

/// <summary>
/// Minimal CRUD contract used by the repository-driven CRUD command base classes.
/// Filtering, pagination, and search are intentionally out of scope; consumers requiring more
/// should derive from the abstract command bases instead.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
/// <typeparam name="TKey">The key type used to identify entities.</typeparam>
[PublicAPI]
public interface ICrudRepository<TEntity, in TKey>
{
    /// <summary>Returns all entities.</summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    Task<IReadOnlyList<TEntity>> ListAsync(CancellationToken cancellationToken);

    /// <summary>Returns the entity with the given key, or <see langword="null"/> if not found.</summary>
    /// <param name="key">The entity key.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    Task<TEntity?> GetAsync(TKey key, CancellationToken cancellationToken);

    /// <summary>Persists a new entity and returns the persisted form.</summary>
    /// <param name="entity">The entity to add.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>Updates an existing entity and returns the persisted form.</summary>
    /// <param name="key">The key of the entity to update.</param>
    /// <param name="entity">The new entity values.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    Task<TEntity> UpdateAsync(TKey key, TEntity entity, CancellationToken cancellationToken);

    /// <summary>Removes the entity with the given key.</summary>
    /// <param name="key">The key of the entity to remove.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    Task RemoveAsync(TKey key, CancellationToken cancellationToken);
}
