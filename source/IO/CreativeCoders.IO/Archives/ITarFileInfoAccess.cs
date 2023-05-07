namespace CreativeCoders.IO.Archives;

public interface ITarFileInfoAccess
{
    int GetFileMode(string path);

    TarFileOwnerInfo GetOwner(string path);
}
