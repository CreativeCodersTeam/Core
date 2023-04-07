namespace CreativeCoders.IO;

public interface IZipArchiveCreator
{
    IZipArchiveCreator SetArchiveFileName(string archiveFileName);

    IZipArchiveCreator AddFromDirectory(string path, string searchPattern, bool recursive);

    void Create();
}
