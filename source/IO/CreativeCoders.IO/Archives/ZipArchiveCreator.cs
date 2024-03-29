using CreativeCoders.Core.IO;
using SharpCompress.Archives;
using SharpCompress.Archives.Zip;

namespace CreativeCoders.IO.Archives;

public class ZipArchiveCreator : IZipArchiveCreator
{
    private readonly ZipArchive _archive = ZipArchive.Create();

    private string? _archiveFileName;

    public IZipArchiveCreator SetArchiveFileName(string archiveFileName)
    {
        _archiveFileName = archiveFileName;

        return this;
    }

    public IZipArchiveCreator AddFromDirectory(string path, string searchPattern, bool recursive)
    {
        _archive.AddAllFromDirectory(path, searchPattern,
            recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

        return this;
    }

    public void Create()
    {
        ArgumentNullException.ThrowIfNull(_archiveFileName);

        using var outputStream = FileSys.File.Create(_archiveFileName);

        _archive.SaveTo(outputStream);
    }
}
