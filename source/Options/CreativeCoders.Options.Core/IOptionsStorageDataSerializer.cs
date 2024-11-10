namespace CreativeCoders.Options.Core;

public interface IOptionsStorageDataSerializer
{
    string Serialize<T>(T options)
        where T : class;

    void Deserialize<T>(string data, T options)
        where T : class;
}
