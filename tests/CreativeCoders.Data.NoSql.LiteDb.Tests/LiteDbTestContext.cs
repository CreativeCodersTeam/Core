using CreativeCoders.Core.IO;
using Microsoft.Extensions.DependencyInjection;

namespace CreativeCoders.Data.NoSql.LiteDb.Tests;

public sealed class LiteDbTestContext<T, TKey> : IDisposable
    where T : class, IDocumentKey<TKey>
    where TKey : IEquatable<TKey>
{
    private readonly FileCleanUp _dbFile;

    private readonly ServiceProvider _serviceProvider;

    public LiteDbTestContext(Action<ILiteCollectionIndexBuilder<T>>? indexBuilder = null)
    {
        var services = new ServiceCollection();

        _dbFile = new FileCleanUp(FileSys.Path.Combine(FileSys.Path.GetTempPath(), FileSys.Path.GetRandomFileName()));

        if (indexBuilder == null)
        {
            services.AddLiteDbDocumentRepositories(_dbFile.FileName)
                .AddRepository<T, TKey>("test");
        }
        else
        {
            services.AddLiteDbDocumentRepositories(_dbFile.FileName)
                .AddRepository<T, TKey>(indexBuilder, "test");
        }

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
