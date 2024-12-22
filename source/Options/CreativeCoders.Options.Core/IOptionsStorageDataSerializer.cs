namespace CreativeCoders.Options.Core;

/// <summary>
///     Defines methods for serializing and deserializing options to and from a storage format.
/// </summary>
public interface IOptionsStorageDataSerializer<in T>
    where T : class
{
    string Serialize(T options);

    void Deserialize(string data, T options);
}
