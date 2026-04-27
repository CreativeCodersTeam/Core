using JetBrains.Annotations;

namespace CreativeCoders.Cli.Commands.Crud;

/// <summary>
/// Optional marker interface for option types that carry an entity payload directly. When not
/// implemented, Add and Update command bases fall back to the <c>BuildEntityAsync</c> hook.
/// </summary>
/// <typeparam name="TEntity">The entity type.</typeparam>
[PublicAPI]
public interface IEntityInputOptions<TEntity>
{
    /// <summary>Gets or sets the entity payload.</summary>
    TEntity Entity { get; set; }
}
