using CreativeCoders.Core.IO;
using SharpCompress.Archives;
using SharpCompress.Archives.Tar;
using SharpCompress.Common;
using SharpCompress.Writers.GZip;
using SharpCompress.Writers.Tar;

namespace CreativeCoders.IO.Archives;

public class TarArchiveCreator : ITarArchiveCreator
{
    private readonly TarArchive _archive = TarArchive.Create();

    private string? _archiveFileName;

    public ITarArchiveCreator SetArchiveFileName(string archiveFileName)
    {
        _archiveFileName = archiveFileName;

        return this;
    }

    public ITarArchiveCreator AddFromDirectory(string path, string searchPattern, bool recursive)
    {
        _archive.AddAllFromDirectory(path, searchPattern,
            recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

        return this;
    }

    public void Create()
    {
        Create(false);
    }

    public void Create(bool gzipArchive)
    {
        ArgumentNullException.ThrowIfNull(_archiveFileName);

        using var outputStream = FileSys.File.Create(_archiveFileName);

        _archive.SaveTo(
                outputStream,
                gzipArchive
                    ? new GZipWriterOptions()
                    : new TarWriterOptions(CompressionType.None, true));
    }
}
