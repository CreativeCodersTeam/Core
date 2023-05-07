using System.IO.Compression;

namespace CreativeCoders.IO.Archives;

public class DefaultTarArchiveWriterBuilder : ITarArchiveWriterBuilder
{
    private bool _withGZip;

    private bool _withOwnerAndGroup;

    private bool _preserveFileMode;

    public ITarArchiveWriterBuilder WithGZip()
    {
        _withGZip = true;

        return this;
    }

    public ITarArchiveWriterBuilder WithOwnerAndGroup()
    {
        _withOwnerAndGroup = true;

        return this;
    }

    public ITarArchiveWriterBuilder PreserveFileMode()
    {
        _preserveFileMode = true;

        return this;
    }

    public ITarArchiveWriter Build(Stream outputStream)
    {
        var tarStream = _withGZip
            ? new GZipStream(outputStream, CompressionLevel.Optimal)
            : outputStream;

        return new DefaultTarArchiveWriter(tarStream, _preserveFileMode, _withOwnerAndGroup);
    }
}
