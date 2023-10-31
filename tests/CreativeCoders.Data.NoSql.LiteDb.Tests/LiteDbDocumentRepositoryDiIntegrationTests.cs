using CreativeCoders.Core.IO;
using CreativeCoders.Data.NoSql.LiteDb.Tests.TestData;
using FluentAssertions;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.Data.NoSql.LiteDb.Tests;

public class LiteDbDocumentRepositoryDiIntegrationTests
{
    [Fact]
    public void RegisterService_GetServiceFromProvider_ServiceInstanceIsReturned()
    {
        // Arrange
        var services = new ServiceCollection();

        using var fcu = new FileCleanUp(FileSys.Path.GetTempFileName());
        
        services.AddLiteDbDocumentRepositories(fcu.FileName)
            .AddRepository<TestDocument, string>("test");
        
        using var sp = services.BuildServiceProvider();
        
        // Act
        var repo = sp.GetRequiredService<IDocumentRepository<TestDocument, string>>();

        // Assert
        repo
            .Should()
            .NotBeNull();
    }

    [Fact]
    public async Task AddAsync_EmptyRepoWithStringId_NewDocumentIsInserted()
    {
        // Arrange
        var testDoc = new TestDocument { Id = Guid.NewGuid().ToString(),Name = "Test1234", Version = 1234};

        using var context = new LiteDbTestContext<TestDocument, string>();
        
        // Act
        await context.Repository.AddAsync(testDoc);

        // Assert
        var docs = (await context.Repository.GetAllAsync()).ToArray();
        
        docs
            .Should()
            .HaveCount(1);
        
        var doc = docs.First();

        doc.Should()
            .NotBeSameAs(testDoc);
        
        doc
            .Should()
            .BeEquivalentTo(testDoc);
    }
    
    [Fact]
    public async Task AddAsync_EmptyRepoWithObjectId_NewDocumentIsInserted()
    {
        // Arrange
        var testDoc = new TestDocumentWithObjectId { Name = "Test1234", Version = 1234};

        using var context = new LiteDbTestContext<TestDocumentWithObjectId, ObjectId>();
        
        // Act
        await context.Repository.AddAsync(testDoc);

        // Assert
        var docs = (await context.Repository.GetAllAsync()).ToArray();
        
        docs
            .Should()
            .HaveCount(1);
        
        var doc = docs.First();

        doc.Should()
            .NotBeSameAs(testDoc);

        doc.Id
            .Should()
            .NotBeNull()
            .And
            .NotBe(ObjectId.Empty);
            
        
        doc
            .Should()
            .BeEquivalentTo(testDoc);
    }
}