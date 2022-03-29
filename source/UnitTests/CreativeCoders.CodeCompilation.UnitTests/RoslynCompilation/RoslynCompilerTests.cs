using System.IO;
using System.Linq;
using System.Reflection;
using CreativeCoders.CodeCompilation.Roslyn;
using CreativeCoders.CodeCompilation.UnitTests.TestData;
using Xunit;

namespace CreativeCoders.CodeCompilation.UnitTests.RoslynCompilation;

public class RoslynCompilerTests
{
    [Fact]
    public void Compile_SimpleTestScript_AddMethodReturnsCorrectValue()
    {
        var compiler = new RoslynCompiler();

        var outputStream = new MemoryStream();

        var compilationPackage = new CompilationPackage();

        compilationPackage.AddReferenceAssembly(typeof(ISimpleScript).Assembly);
        compilationPackage.SourceCodes.Add(new SourceCodeUnit(TestScripts.SimpleScript, "Test.cs"));
            
        var compilationOutput = new CompilationOutput(CompilationOutputKind.DynamicallyLinkedLibrary,
            new StreamCompilationOutputData(outputStream));

        var compilerResult = compiler.Compile(compilationPackage, compilationOutput);

        Assert.Empty(compilerResult.Messages.Where(x => x.MessageType == CompilationMessageType.Error));
            
        var assembly = Assembly.Load(outputStream.ToArray());

        var simpleScript = assembly.CreateInstance(
            "CreativeCoders.CodeCompilation.UnitTests.TestData.SimpleScript") as ISimpleScript;

        var value = simpleScript?.AddIntegers(12, 34);

        Assert.Equal(46, value);
    }
}