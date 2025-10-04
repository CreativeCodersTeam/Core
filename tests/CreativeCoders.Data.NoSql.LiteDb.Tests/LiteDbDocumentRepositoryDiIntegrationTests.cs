using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Core.IO;
using CreativeCoders.Data.NoSql.LiteDb.Tests.TestData;
using AwesomeAssertions;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.Data.NoSql.LiteDb.Tests;

public class LiteDbDocumentRepositoryDiIntegrationTests
{
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

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task AddAsync_RepoWithOneEntityWithSameId_ExceptionIsThrown()
    {
        // Arrange
        var testDoc = new TestDocument { Id = Guid.NewGuid().ToString(),Name = "Test1234", Version = 1234};

        using var context = new LiteDbTestContext<TestDocument, string>();

        await context.Repository.AddAsync(testDoc);

        // Act
        var act = async () => await context.Repository.AddAsync(testDoc);

        // Assert
        await act
            .Should()
            .ThrowAsync<LiteException>();
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task AddAsync_RepoWithOneEntityWithUniqueIndexViolation_ExceptionIsThrown()
    {
        // Arrange
        var testDoc = new TestDocument { Id = Guid.NewGuid().ToString(),Name = "Test1234", Version = 1234};

        using var context = new LiteDbTestContext<TestDocument, string>(
            indexBuilder => indexBuilder.AddIndex(x => x.Name, true));

        await context.Repository.AddAsync(testDoc);

        // Act
        testDoc.Id = Guid.NewGuid().ToString();
        var act = async () => await context.Repository.AddAsync(testDoc);

        // Assert
        await act
            .Should()
            .ThrowAsync<LiteException>()
            .WithMessage("*unique index 'Name'*");
    }

    [Fact]
    public async Task QueryAsync_RepoWithOneEntityQueriedByExistingId_QueryReturnsThisDocument()
    {
        // Arrange
        var testDoc = new TestDocument { Id = Guid.NewGuid().ToString(),Name = "Test1234", Version = 1234};

        using var context = new LiteDbTestContext<TestDocument, string>(
            indexBuilder => indexBuilder.AddIndex(x => x.Name, true));

        await context.Repository.AddAsync(testDoc);

        // Act
        var docs = (await context.Repository.QueryAsync(x => x.Id == testDoc.Id)).ToArray();

        // Assert
        docs
            .Should()
            .HaveCount(1);

        docs.First()
            .Should()
            .NotBeSameAs(testDoc)
            .And
            .BeEquivalentTo(testDoc);
    }

    [Fact]
    public async Task QueryAsync_RepoWithTwoEntityQueriedByOneExistingId_QueryReturnsOneDocument()
    {
        // Arrange
        var testDoc0 = new TestDocument { Id = Guid.NewGuid().ToString(),Name = "Test1234", Version = 1234};
        var testDoc1 = new TestDocument { Id = Guid.NewGuid().ToString(),Name = "TestQwertz", Version = 6789};

        using var context = new LiteDbTestContext<TestDocument, string>(
            indexBuilder => indexBuilder.AddIndex(x => x.Name, true));

        await context.Repository.AddAsync(testDoc0);
        await context.Repository.AddAsync(testDoc1);

        // Act
        var docs = (await context.Repository.QueryAsync(x => x.Id == testDoc1.Id)).ToArray();

        // Assert
        docs
            .Should()
            .HaveCount(1);

        docs.First()
            .Should()
            .NotBeSameAs(testDoc1)
            .And
            .BeEquivalentTo(testDoc1);
    }

    [Fact]
    public async Task QueryAsync_RepoWithMultipleEntitiesQueriedByName_QueryReturnsMultipleDocuments()
    {
        // Arrange
        var testDoc0 = new TestDocument { Id = Guid.NewGuid().ToString(), Name = "Test1234", Version = 1234 };
        var testDoc1 = new TestDocument
            { Id = Guid.NewGuid().ToString(), Name = "TestQwertz", Version = 6789 };
        var testDoc2 = new TestDocument { Id = Guid.NewGuid().ToString(), Name = "ABCD", Version = 3456 };
        var testDoc3 = new TestDocument { Id = Guid.NewGuid().ToString(), Name = "1234", Version = 5678 };

        using var context = new LiteDbTestContext<TestDocument, string>(
            indexBuilder => indexBuilder.AddIndex(x => x.Name, true));

        await context.Repository.AddAsync(testDoc0);
        await context.Repository.AddAsync(testDoc1);
        await context.Repository.AddAsync(testDoc2);
        await context.Repository.AddAsync(testDoc3);

        // Act
        var docsWithTest = (await context.Repository.QueryAsync(
                x => x.Name != null && x.Name.StartsWith("Test", StringComparison.Ordinal)))
            .ToArray();
        var docsWith1234 = (await context.Repository.QueryAsync(
                x => x.Name != null && x.Name.Contains("1234", StringComparison.Ordinal)))
            .ToArray();

        // Assert
        docsWithTest
            .Should()
            .HaveCount(2)
            .And
            .BeEquivalentTo(new[] { testDoc0, testDoc1 });

        docsWith1234
            .Should()
            .HaveCount(2)
            .And
            .BeEquivalentTo(new[] { testDoc0, testDoc3 });
    }

    [Fact]
    public async Task GetAsync_RepoWithOneEntityQueriedByExistingId_QueryReturnsThisDocument()
    {
        // Arrange
        var testDoc = new TestDocument { Id = Guid.NewGuid().ToString(),Name = "Test1234", Version = 1234};

        using var context = new LiteDbTestContext<TestDocument, string>(
            indexBuilder => indexBuilder.AddIndex(x => x.Name, true));

        await context.Repository.AddAsync(testDoc);

        // Act
        var doc = await context.Repository.GetAsync(testDoc.Id);

        // Assert
        doc
            .Should()
            .NotBeSameAs(testDoc)
            .And
            .BeEquivalentTo(testDoc);
    }

    [Fact]
    public async Task DeleteAsync_RepoWithMultipleEntitiesDeleteOne_DeletedEntryMissing()
    {
        // Arrange
        var testDoc0 = new TestDocument { Id = Guid.NewGuid().ToString(), Name = "Test1234", Version = 1234 };
        var testDoc1 = new TestDocument
            { Id = Guid.NewGuid().ToString(), Name = "TestQwertz", Version = 6789 };
        var testDoc2 = new TestDocument { Id = Guid.NewGuid().ToString(), Name = "ABCD", Version = 3456 };
        var testDoc3 = new TestDocument { Id = Guid.NewGuid().ToString(), Name = "1234", Version = 5678 };

        using var context = new LiteDbTestContext<TestDocument, string>(
            indexBuilder => indexBuilder.AddIndex(x => x.Name, true));

        await context.Repository.AddAsync(testDoc0);
        await context.Repository.AddAsync(testDoc1);
        await context.Repository.AddAsync(testDoc2);
        await context.Repository.AddAsync(testDoc3);

        // Act
        await context.Repository.DeleteAsync(testDoc1.Id);

        // Assert
        var allDocs = (await context.Repository.GetAllAsync()).ToArray();

        allDocs
            .Should()
            .HaveCount(3)
            .And
            .BeEquivalentTo(new[] { testDoc0, testDoc2, testDoc3 });
    }

    [Fact]
    public async Task DeleteAsync_RepoWithMultipleEntitiesDeleteMany_DeletedEntriesMissing()
    {
        // Arrange
        var testDoc0 = new TestDocument { Id = Guid.NewGuid().ToString(), Name = "Test1234", Version = 1234 };
        var testDoc1 = new TestDocument
            { Id = Guid.NewGuid().ToString(), Name = "TestQwertz", Version = 6789 };
        var testDoc2 = new TestDocument { Id = Guid.NewGuid().ToString(), Name = "ABCD", Version = 3456 };
        var testDoc3 = new TestDocument { Id = Guid.NewGuid().ToString(), Name = "1234", Version = 5678 };

        using var context = new LiteDbTestContext<TestDocument, string>(
            indexBuilder => indexBuilder.AddIndex(x => x.Name, true));

        await context.Repository.AddAsync(testDoc0);
        await context.Repository.AddAsync(testDoc1);
        await context.Repository.AddAsync(testDoc2);
        await context.Repository.AddAsync(testDoc3);

        // Act
        await context.Repository.DeleteAsync(x => x.Name != null && x.Name.Contains("1234"));

        // Assert
        var allDocs = (await context.Repository.GetAllAsync()).ToArray();

        allDocs
            .Should()
            .HaveCount(2)
            .And
            .BeEquivalentTo(new[] { testDoc1, testDoc2 });
    }

    [Fact]
    public async Task ClearAsync_RepoWithMultipleEntities_RepoIsEmpty()
    {
        // Arrange
        var testDoc0 = new TestDocument { Id = Guid.NewGuid().ToString(), Name = "Test1234", Version = 1234 };
        var testDoc1 = new TestDocument
            { Id = Guid.NewGuid().ToString(), Name = "TestQwertz", Version = 6789 };
        var testDoc2 = new TestDocument { Id = Guid.NewGuid().ToString(), Name = "ABCD", Version = 3456 };
        var testDoc3 = new TestDocument { Id = Guid.NewGuid().ToString(), Name = "1234", Version = 5678 };

        using var context = new LiteDbTestContext<TestDocument, string>(
            indexBuilder => indexBuilder.AddIndex(x => x.Name, true));

        await context.Repository.AddAsync(testDoc0);
        await context.Repository.AddAsync(testDoc1);
        await context.Repository.AddAsync(testDoc2);
        await context.Repository.AddAsync(testDoc3);

        // Act
        await context.Repository.ClearAsync();

        // Assert
        var allDocs = (await context.Repository.GetAllAsync()).ToArray();

        allDocs
            .Should()
            .BeEmpty();
    }

    [Fact]
    public async Task UpdateAsync_UpdateOneDoc_DocIsUpdated()
    {
        // Arrange
        var testDoc0 = new TestDocument { Id = Guid.NewGuid().ToString(), Name = "Test1234", Version = 1234 };
        var testDoc1 = new TestDocument
            { Id = Guid.NewGuid().ToString(), Name = "TestQwertz", Version = 6789 };
        var testDoc2 = new TestDocument { Id = Guid.NewGuid().ToString(), Name = "ABCD", Version = 3456 };
        var testDoc3 = new TestDocument { Id = Guid.NewGuid().ToString(), Name = "1234", Version = 5678 };

        using var context = new LiteDbTestContext<TestDocument, string>(
            indexBuilder => indexBuilder.AddIndex(x => x.Name, true));

        await context.Repository.AddAsync(testDoc0);
        await context.Repository.AddAsync(testDoc1);
        await context.Repository.AddAsync(testDoc2);
        await context.Repository.AddAsync(testDoc3);

        // Act
        testDoc1.Name = "TestQwertzUpdated";
        await context.Repository.UpdateAsync(testDoc1);

        // Assert
        var doc = await context.Repository.GetAsync(testDoc1.Id);

        doc
            .Should()
            .NotBeSameAs(testDoc1)
            .And
            .BeEquivalentTo(testDoc1);
    }

    [Fact]
    [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
    public async Task UpdateAsync_IdIsNull_ExceptionIsThrown()
    {
        // Arrange
        var testDoc0 = new TestDocument { Id = Guid.NewGuid().ToString(), Name = "Test1234", Version = 1234 };

        using var context = new LiteDbTestContext<TestDocument, string>(
            indexBuilder => indexBuilder.AddIndex(x => x.Name, true));

        await context.Repository.AddAsync(testDoc0);

        // Act
        testDoc0.Id = null;
        var act = async() => await context.Repository.UpdateAsync(testDoc0);

        // Assert
        await act
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Entity has no id");
    }
}
