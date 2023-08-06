using System.Text;
using CreativeCoders.Core;
using CreativeCoders.Core.IO;
using ICSharpCode.SharpZipLib.Tar;
using JetBrains.Annotations;

namespace CreativeCoders.IO.Archives;

[PublicAPI]
public sealed class SharpZipLibTarArchiveWriter : TarArchiveWriterBase
{
    private readonly Stream _tarStream;

    private readonly TarOutputStream _tarOutputStream;

    public SharpZipLibTarArchiveWriter(Stream tarStream, bool preserveFileMode, bool withOwnerAndGroup)
        : base(preserveFileMode, withOwnerAndGroup)
    {
        _tarStream = Ensure.NotNull(tarStream);

        _tarOutputStream = new TarOutputStream(_tarStream, Encoding.ASCII);
    }

    public override async ValueTask DisposeAsync()
    {
        await _tarOutputStream.DisposeAsync().ConfigureAwait(false);
        await _tarStream.DisposeAsync().ConfigureAwait(false);
    }

    public override async Task AddFileAsync(string fileNameInArchive, Stream fileContent, long contentSize, int fileMode,
        TarFileOwnerInfo fileOwnerInfo, DateTime modificationTime)
    {
        var tarEntry = TarEntry.CreateTarEntry(fileNameInArchive.Replace(FileSys.Path.DirectorySeparatorChar, '/'));

        tarEntry.UserName = fileOwnerInfo.UserName;
        tarEntry.UserId = fileOwnerInfo.UserId;
        tarEntry.GroupName = fileOwnerInfo.GroupName;
        tarEntry.GroupId = fileOwnerInfo.GroupId;
        tarEntry.TarHeader.Mode = fileMode;
        tarEntry.ModTime = modificationTime.ToUniversalTime();

        tarEntry.Size = contentSize;

        await _tarOutputStream.PutNextEntryAsync(tarEntry, CancellationToken.None).ConfigureAwait(false);

        await fileContent.CopyToAsync(_tarOutputStream).ConfigureAwait(false);

        await _tarOutputStream.CloseEntryAsync(CancellationToken.None).ConfigureAwait(false);
    }
}
