using System.Text;
using CreativeCoders.Core;
using ICSharpCode.SharpZipLib.Tar;

namespace CreativeCoders.IO.Archives;

public class SharpZipLibTarArchiveWriter : TarArchiveWriterBase
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
        TarFileOwnerInfo fileOwnerInfo)
    {
        var tarEntry = TarEntry.CreateTarEntry(fileNameInArchive);

        tarEntry.Size = contentSize;

        await _tarOutputStream.PutNextEntryAsync(tarEntry, CancellationToken.None).ConfigureAwait(false);

        await fileContent.CopyToAsync(_tarOutputStream).ConfigureAwait(false);

        await _tarOutputStream.CloseEntryAsync(CancellationToken.None).ConfigureAwait(false);
    }
}
