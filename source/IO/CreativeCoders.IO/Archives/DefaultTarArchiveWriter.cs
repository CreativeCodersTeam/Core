using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.Core.IO;
using TapeArchive;

namespace CreativeCoders.IO.Archives;

public class DefaultTarArchiveWriter : ITarArchiveWriter
{
    private readonly Stream _tarStream;

    private readonly bool _preserveFileMode;

    private readonly bool _withOwnerAndGroup;

    private readonly ArchiveBuilder _archiveBuilder;

    public DefaultTarArchiveWriter(Stream tarStream, bool preserveFileMode, bool withOwnerAndGroup)
    {
        _tarStream = Ensure.NotNull(tarStream);
        _preserveFileMode = preserveFileMode;
        _withOwnerAndGroup = withOwnerAndGroup;

        _archiveBuilder = new ArchiveBuilder(_tarStream, true);
    }

    public async ValueTask DisposeAsync()
    {
        await _archiveBuilder.CompleteAsync().ConfigureAwait(false);

        await _archiveBuilder.DisposeAsync().ConfigureAwait(false);

        await _tarStream.DisposeAsync().ConfigureAwait(false);
    }

    public async Task AddFileAsync(string fileName, string fileNameInArchive)
    {
        var (userId, groupId) = GetOwner();

        await using var fs = FileSys.File.OpenRead(fileName);

        await _archiveBuilder.WriteItemAsync(
                new UstarItem(PrePosixType.RegularFile, ItemName.FromFileSystem(fileNameInArchive, false))
                {
                    Content = fs,
                    Size = fs.Length,
                    Mode = GetFileMode(fileName),
                    UserId = userId,
                    GroupId = groupId
                },
                null)
            .ConfigureAwait(false);
    }

    public Task AddFromDirectoryAsync(string path)
    {
        return AddFromDirectoryAsync(path, string.Empty);
    }

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

    private static string RemovePrefix(string path, string prefix)
    {
        if (string.IsNullOrEmpty(prefix) || !path.StartsWith(prefix, StringComparison.CurrentCulture))
        {
            return path;
        }

        return path[prefix.Length..];
    }

    private int GetFileMode(string fileName)
    {
        return !_preserveFileMode ? 0 : 0;
    }

    private (int userId, int groupId) GetOwner()
    {
        return !_withOwnerAndGroup ? (0, 0) : (0, 0);
    }
}
