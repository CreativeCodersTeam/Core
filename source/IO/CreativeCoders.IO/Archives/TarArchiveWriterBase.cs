using CreativeCoders.Core.Collections;
using CreativeCoders.Core.IO;

namespace CreativeCoders.IO.Archives;

public abstract class TarArchiveWriterBase : ITarArchiveWriter
{
    private readonly bool _preserveFileMode;

    private readonly bool _withOwnerAndGroup;

    private readonly ITarFileInfoAccess _tarFileInfoAccess;

    protected TarArchiveWriterBase(bool preserveFileMode, bool withOwnerAndGroup)
    {
        _preserveFileMode = preserveFileMode;
        _withOwnerAndGroup = withOwnerAndGroup;

        _tarFileInfoAccess = new WindowsTarFileInfoAccess();
    }

    public abstract ValueTask DisposeAsync();

    public async Task AddFileAsync(string fileName, string fileNameInArchive)
    {
        var fs = FileSys.File.OpenRead(fileName);
        await using var _ = fs.ConfigureAwait(false);

        await AddFileAsync(fileNameInArchive, fs, fs.Length, GetFileMode(fileName), GetOwner(fileName),
                _tarFileInfoAccess.GetModificationTime(fileName))
            .ConfigureAwait(false);
    }

    public abstract Task AddFileAsync(string fileNameInArchive, Stream fileContent, long contentSize,
        int fileMode, TarFileOwnerInfo fileOwnerInfo, DateTime modificationTime);

    public async Task AddFromDirectoryAsync(string path, string removePrefix)
    {
        await FileSys
            .Directory
            .EnumerateFiles(path)
            .ForEachAsync(async x => await AddFileAsync(x, RemovePrefix(x, removePrefix)).ConfigureAwait(false))
            .ConfigureAwait(false);

        await FileSys
            .Directory
            .EnumerateDirectories(path)
            .ForEachAsync(async x => await AddFromDirectoryAsync(x, removePrefix).ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    public Task AddFromDirectoryAsync(string path)
    {
        return AddFromDirectoryAsync(path, string.Empty);
    }

    protected static string RemovePrefix(string path, string prefix)
    {
        if (string.IsNullOrEmpty(prefix) || !path.StartsWith(prefix, StringComparison.CurrentCulture))
        {
            return path;
        }

        return path[prefix.Length..];
    }

    protected int GetFileMode(string fileName)
    {
        return _preserveFileMode
            ? _tarFileInfoAccess.GetFileMode(fileName)
            : 0;
    }

    protected TarFileOwnerInfo GetOwner(string fileName)
    {
        return _withOwnerAndGroup
            ? _tarFileInfoAccess.GetOwner(fileName)
            : new TarFileOwnerInfo();
    }
}
