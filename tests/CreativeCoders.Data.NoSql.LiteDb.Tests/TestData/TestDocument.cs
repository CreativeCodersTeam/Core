namespace CreativeCoders.Data.NoSql.LiteDb.Tests.TestData;

public class TestDocument : IDocumentKey<string>
{
    public string? Id { get; set; }
    
    public string? Name { get; set; }

    public int Version { get; set; }
}