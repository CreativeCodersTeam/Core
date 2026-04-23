namespace CreativeCoders.Core.Weak;

/// <summary>
/// Specifies how the owner of a weak reference wrapper is kept alive.
/// </summary>
public enum KeepOwnerAliveMode
{
    /// <summary>
    /// The owner is always kept alive by maintaining a strong reference.
    /// </summary>
    KeepAlive,

    /// <summary>
    /// The owner is not kept alive; only a weak reference is held.
    /// </summary>
    NotKeepAlive,

    /// <summary>
    /// The keep-alive behavior is determined automatically based on whether the owner is a compiler-generated closure type.
    /// </summary>
    AutoGuess
}
