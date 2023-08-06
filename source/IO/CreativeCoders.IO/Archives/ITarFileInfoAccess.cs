using JetBrains.Annotations;

namespace CreativeCoders.IO.Archives;

[PublicAPI]
public interface ITarFileInfoAccess
{
    int GetFileMode(string path);

    TarFileOwnerInfo GetOwner(string path);

    DateTime GetModificationTime(string path);
}
