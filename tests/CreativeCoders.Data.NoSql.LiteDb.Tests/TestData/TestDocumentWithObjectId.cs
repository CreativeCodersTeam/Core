using System.Diagnostics.CodeAnalysis;
using LiteDB;

namespace CreativeCoders.Data.NoSql.LiteDb.Tests.TestData;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public class TestDocumentWithObjectId : IDocumentKey<ObjectId>
{
    public ObjectId? Id { get; set; }

    public string? Name { get; set; }

    public int Version { get; set; }
}
