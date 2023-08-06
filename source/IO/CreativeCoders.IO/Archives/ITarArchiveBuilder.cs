using JetBrains.Annotations;

namespace CreativeCoders.IO.Archives;

[PublicAPI]
public interface ITarArchiveWriterBuilder
{
    ITarArchiveWriterBuilder WithGZip();

    ITarArchiveWriterBuilder WithOwnerAndGroup();

    ITarArchiveWriterBuilder PreserveFileMode();

    ITarArchiveWriter Build(Stream outputStream);
}
