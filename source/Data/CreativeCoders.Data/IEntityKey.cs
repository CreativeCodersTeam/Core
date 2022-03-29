using JetBrains.Annotations;

namespace CreativeCoders.Data;

[PublicAPI]
public interface IEntityKey<out TKey>
{
    TKey Id { get; }
}
