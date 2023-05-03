namespace CreativeCoders.IO.Archives;

public class DefaultTarArchiveWriterBuilder : ITarArchiveWriterBuilder
{
    public ITarArchiveWriterBuilder WithGZip()
    {
        throw new NotImplementedException();
    }

    public ITarArchiveWriterBuilder WithOwnerAndGroup()
    {
        throw new NotImplementedException();
    }

    public ITarArchiveWriterBuilder PreserveFileMode()
    {
        throw new NotImplementedException();
    }

    public ITarArchiveWriter Build(Stream outputStream)
    {
        throw new NotImplementedException();
    }
}
