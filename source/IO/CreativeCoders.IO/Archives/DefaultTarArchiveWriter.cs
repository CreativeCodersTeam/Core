using CreativeCoders.Core;
using TapeArchive;

namespace CreativeCoders.IO.Archives;

public class DefaultTarArchiveWriter : TarArchiveWriterBase
{
    private readonly Stream _tarStream;

    private readonly ArchiveBuilder _archiveBuilder;

    public DefaultTarArchiveWriter(Stream tarStream, bool preserveFileMode, bool withOwnerAndGroup)
        : base(preserveFileMode, withOwnerAndGroup)
    {
        _tarStream = Ensure.NotNull(tarStream);

        _archiveBuilder = new ArchiveBuilder(_tarStream, true);
    }

    public override async ValueTask DisposeAsync()
    {
        await _archiveBuilder.CompleteAsync().ConfigureAwait(false);

        await _archiveBuilder.DisposeAsync().ConfigureAwait(false);

        await _tarStream.DisposeAsync().ConfigureAwait(false);
    }

    // public async Task AddFileAsync(string fileName, string fileNameInArchive)
    // {
    //     var ownerInfo = GetOwner(fileName);
    //
    //     await using var fs = FileSys.File.OpenRead(fileName);
    //
    //     await AddFileAsync(fileNameInArchive, fs, fs.Length, GetFileMode(fileName), GetOwner(fileName))
    //         .ConfigureAwait(false);
    // }

    public override async Task AddFileAsync(string fileNameInArchive, Stream fileContent, long contentSize,
        int fileMode, TarFileOwnerInfo fileOwnerInfo)
    {
        await _archiveBuilder.WriteItemAsync(
                new UstarItem(PrePosixType.RegularFile, ItemName.FromFileSystem(fileNameInArchive, false))
                {
                    Content = fileContent,
                    Size = contentSize,
                    Mode = fileMode,
                    UserId = fileOwnerInfo.UserId,
                    GroupId = fileOwnerInfo.GroupId,
                    UserName = fileOwnerInfo.UserName,
                    GroupName = fileOwnerInfo.GroupName
                },
                null)
            .ConfigureAwait(false);
    }

    // public Task AddFromDirectoryAsync(string path)
    // {
    //     return AddFromDirectoryAsync(path, string.Empty);
    // }
    //
    // public async Task AddFromDirectoryAsync(string path, string removePrefix)
    // {
    //     await FileSys
    //         .Directory
    //         .EnumerateFiles(path)
    //         .ForEachAsync(async x => await AddFileAsync(x, RemovePrefix(x, removePrefix)).ConfigureAwait(false))
    //         .ConfigureAwait(false);
    //
    //     await FileSys
    //         .Directory
    //         .EnumerateDirectories(path)
    //         .ForEachAsync(async x => await AddFromDirectoryAsync(x, removePrefix).ConfigureAwait(false))
    //         .ConfigureAwait(false);
    // }

    // private static string RemovePrefix(string path, string prefix)
    // {
    //     if (string.IsNullOrEmpty(prefix) || !path.StartsWith(prefix, StringComparison.CurrentCulture))
    //     {
    //         return path;
    //     }
    //
    //     return path[prefix.Length..];
    // }
    //
    // private int GetFileMode(string fileName)
    // {
    //     return _preserveFileMode
    //         ? _tarFileInfoAccess.GetFileMode(fileName)
    //         : 0;
    // }
    //
    // private TarFileOwnerInfo GetOwner(string fileName)
    // {
    //     return _withOwnerAndGroup
    //         ? _tarFileInfoAccess.GetOwner(fileName)
    //         : new TarFileOwnerInfo();
    // }
}
