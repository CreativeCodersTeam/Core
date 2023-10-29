namespace CreativeCoders.Data.NoSql;

public interface IDocumentKey<T>
{
    T Id { get; set; }
}
