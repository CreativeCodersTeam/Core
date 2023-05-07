using TapeArchive;

namespace CreativeCoders.IO.Archives;

public class WindowsTarFileInfoAccess : ITarFileInfoAccess
{
    public int GetFileMode(string path)
    {
        return UnixPermissions.GroupExecute |
               UnixPermissions.GroupRead |
               UnixPermissions.GroupWrite |
               UnixPermissions.OwnerExecute |
               UnixPermissions.OwnerRead |
               UnixPermissions.OwnerWrite |
               UnixPermissions.OtherExecute |
               UnixPermissions.OtherRead |
               UnixPermissions.OtherWrite;
    }

    public TarFileOwnerInfo GetOwner(string path)
    {
        return new TarFileOwnerInfo
        {
            UserName = Environment.UserName
        };
    }
}
