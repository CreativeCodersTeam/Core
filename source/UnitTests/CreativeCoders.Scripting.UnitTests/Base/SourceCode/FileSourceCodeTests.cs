using System.IO.Abstractions.TestingHelpers;
using CreativeCoders.Scripting.Base.SourceCode;
using CreativeCoders.UnitTests;
using Xunit;

namespace CreativeCoders.Scripting.UnitTests.Base.SourceCode;

[Collection("FileSys")]
public class FileSourceCodeTests
{
    [Fact]
    public void Read_WithFileName_ReturnsFileContent()
    {
        const string fileContent = "public void Test(){}";
        const string fileName = @"c:\temp\test.cs";

        var fileSystem = new MockFileSystemEx();
        fileSystem.AddFile(fileName, new MockFileData(fileContent));
        fileSystem.Install();
            
        var sourceCode = new FileSourceCode(fileName);
            
        Assert.Equal(fileContent, sourceCode.Read());
    }
}