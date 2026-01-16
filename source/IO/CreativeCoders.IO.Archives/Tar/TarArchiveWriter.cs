using System.Formats.Tar;
using CreativeCoders.Core;

namespace CreativeCoders.IO.Archives.Tar;

public sealed class TarArchiveWriter(Stream outputStream, bool leaveOpen = false) : ITarArchiveWriter
{
    private readonly TarWriter _tarWriter =
        new TarWriter(Ensure.NotNull(outputStream), TarEntryFormat.Pax, leaveOpen);

    public Task AddFileAsync(string fileName, string fileNameInArchive)
    {
        return _tarWriter.WriteEntryAsync(fileName, ConvertPathToTarEntryName(fileNameInArchive));
    }

    public Task AddFileAsync(Stream stream, string fileNameInArchive)
    {
        return AddFileCoreAsync(stream, fileNameInArchive, null);
    }

    private Task AddFileCoreAsync(Stream stream, string fileNameInArchive,
        Action<TarEntryInfo>? configureEntry)
    {
        TarEntryInfo? entryInfo = null;

        if (configureEntry != null)
        {
            entryInfo = new TarEntryInfo();
            configureEntry(entryInfo);
        }

        var tarEntry = CreateTarEntry(stream, fileNameInArchive, entryInfo);

        return _tarWriter.WriteEntryAsync(tarEntry);
    }

    private static PaxTarEntry CreateTarEntry(Stream stream, string fileNameInArchive,
        TarEntryInfo? entryInfo)
    {
        return entryInfo == null
            ? new PaxTarEntry(TarEntryType.RegularFile, ConvertPathToTarEntryName(fileNameInArchive))
            {
                DataStream = stream
            }
            : new PaxTarEntry(TarEntryType.RegularFile, ConvertPathToTarEntryName(fileNameInArchive),
                entryInfo.ExtendedAttributes)
            {
                DataStream = stream,
                UserName = entryInfo.UserName,
                Uid = entryInfo.UserId,
                GroupName = entryInfo.GroupName,
                Gid = entryInfo.GroupId,
                Mode = entryInfo.FileMode
            };
    }

    public async Task AddFromDirectoryAsync(string path, string basePathToRemove)
    {
        var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var entryName = file[basePathToRemove.Length..].TrimStart(Path.DirectorySeparatorChar);
            await AddFileAsync(file, entryName).ConfigureAwait(false);
        }
    }

    private static string ConvertPathToTarEntryName(string fileNameInArchive)
    {
        return Path.DirectorySeparatorChar != '/'
            ? fileNameInArchive.Replace(Path.DirectorySeparatorChar, '/')
            : fileNameInArchive;
    }

    public void Dispose()
    {
        _tarWriter.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _tarWriter.DisposeAsync().ConfigureAwait(false);
    }

    public Task AddFileAsync(Stream stream, string fileNameInArchive, Action<TarEntryInfo> configureEntry)
    {
        return AddFileCoreAsync(stream, fileNameInArchive, configureEntry);
    }

    public Task AddFileAsync(Stream stream, string fileNameInArchive, TarEntryInfo entryInfo)
    {
        return AddFileCoreAsync(stream, fileNameInArchive, x =>
        {
            x.ExtendedAttributes = entryInfo.ExtendedAttributes;
            x.FileMode = entryInfo.FileMode;
            x.GroupId = entryInfo.GroupId;
            x.GroupName = entryInfo.GroupName;
            x.UserId = entryInfo.UserId;
            x.UserName = entryInfo.UserName;
        });
    }
}

public class TarEntryInfo
{
    public string UserName { get; set; } = string.Empty;

    public int UserId { get; set; }

    public string GroupName { get; set; } = string.Empty;

    public int GroupId { get; set; }

    public UnixFileMode FileMode { get; set; }

    public IReadOnlyDictionary<string, string> ExtendedAttributes { get; set; } =
        new Dictionary<string, string>();
}
