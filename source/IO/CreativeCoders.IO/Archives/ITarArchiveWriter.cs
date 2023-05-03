namespace CreativeCoders.IO.Archives;

public interface ITarArchiveWriter : IDisposable, IAsyncDisposable
{

}

public class DefaultTarArchiveWriter : ITarArchiveWriter
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }
}
