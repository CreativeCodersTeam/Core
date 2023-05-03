namespace CreativeCoders.IO.Archives;

public interface ITarArchiveWriterBuilder
{
    ITarArchiveWriterBuilder WithGZip();

    ITarArchiveWriterBuilder WithOwnerAndGroup();

    ITarArchiveWriterBuilder PreserveFileMode();

    ITarArchiveWriter Build(Stream outputStream);
}
