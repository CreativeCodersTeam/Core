namespace CreativeCoders.Options.Core;

public interface IOptionsStorageProvider<T>
    where T : class
{
    Task WriteAsync(string? name, T options);

    void Write(string? name, T options);

    Task ReadAsync(string? name, T options);

    void Read(string? name, T options);
}
