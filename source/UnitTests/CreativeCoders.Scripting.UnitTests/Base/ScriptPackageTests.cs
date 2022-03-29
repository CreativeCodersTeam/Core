using CreativeCoders.Scripting.Base;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Scripting.UnitTests.Base;

public class ScriptPackageTests
{
    [Fact]
    public void Id_CtorArgument_IdIsSet()
    {
        var scriptPackage = new ScriptPackage("TestId", "TestScript", A.Fake<ISourceCode>());
            
        Assert.Equal("TestId", scriptPackage.Id);
    }
        
    [Fact]
    public void Name_CtorArgument_NameIsSet()
    {
        var scriptPackage = new ScriptPackage("TestId", "TestScript", A.Fake<ISourceCode>());
            
        Assert.Equal("TestScript", scriptPackage.Name);
    }
        
    [Fact]
    public void SourceCode_CtorArgument_SourceCodeIsSet()
    {
        var sourceCode = A.Fake<ISourceCode>();
        var scriptPackage = new ScriptPackage("TestId", "TestScript", sourceCode);
            
        Assert.Equal(sourceCode, scriptPackage.SourceCode);
    }
}