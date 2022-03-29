namespace CreativeCoders.Data;

public interface IEntityKey<out TKey>
{
    TKey Id { get; }
}
