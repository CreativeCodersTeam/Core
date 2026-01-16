namespace CreativeCoders.IO.Archives.Tar;

public static class TarFileHelper
{
    public static bool IsGZipFile(string archiveFileName)
    {
        var fileExt = Path.GetExtension(archiveFileName);
        return fileExt.Equals("gz", StringComparison.CurrentCultureIgnoreCase) ||
               fileExt.Equals("tgz", StringComparison.CurrentCultureIgnoreCase) ||
               fileExt.EndsWith(".gz", StringComparison.CurrentCultureIgnoreCase) ||
               fileExt.EndsWith(".tgz", StringComparison.CurrentCultureIgnoreCase);
    }
}
