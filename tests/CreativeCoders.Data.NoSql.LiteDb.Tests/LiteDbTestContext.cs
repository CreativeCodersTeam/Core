using CreativeCoders.Core.IO;
using CreativeCoders.Data.NoSql.LiteDb.Tests.TestData;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Data.NoSql.LiteDb.Tests;

public sealed class LiteDbTestContext<T, TKey> : IDisposable
    where T : class, IDocumentKey<TKey>
{
    private readonly FileCleanUp _dbFile;

    private readonly ServiceProvider _serviceProvider;

    public LiteDbTestContext()
    {
        var services = new ServiceCollection();

        _dbFile = new FileCleanUp(FileSys.Path.GetTempFileName());
        
        services.AddLiteDbDocumentRepositories(_dbFile.FileName)
            .AddRepository<T, TKey>("test");
        
        _serviceProvider = services.BuildServiceProvider();
        
        Repository = _serviceProvider.GetRequiredService<IDocumentRepository<T, TKey>>();
    }
    
    public void Dispose()
    {
        _serviceProvider.Dispose();
        
        _dbFile.Dispose();
    }
    
    public IDocumentRepository<T, TKey> Repository { get; }
}