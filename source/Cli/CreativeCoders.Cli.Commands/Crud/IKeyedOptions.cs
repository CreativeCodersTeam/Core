using JetBrains.Annotations;

namespace CreativeCoders.Cli.Commands.Crud;

/// <summary>
/// Marker interface implemented by option types that carry a key identifying a single entity.
/// Used by Get / Update / Remove command bases.
/// </summary>
/// <typeparam name="TKey">The key type.</typeparam>
[PublicAPI]
public interface IKeyedOptions<TKey>
{
    /// <summary>Gets or sets the entity key.</summary>
    TKey Key { get; set; }
}
