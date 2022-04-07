using System.Collections.Generic;
using CreativeCoders.CodeCompilation.Roslyn;
using CreativeCoders.Core.Collections;
using CreativeCoders.Scripting.CSharp;
using CreativeCoders.Scripting.CSharp.ClassTemplating;

namespace CreativeCoders.Scripting.UnitTests.CSharp;

public class TestScriptImplementation : CSharpScriptImplementation
{
    public TestScriptImplementation(IEnumerable<ISourcePreprocessor> sourcePreprocessors, ITestApi testApi,
        bool scriptObjectWithInterface)
        : base(CreateScriptClassTemplate(scriptObjectWithInterface, testApi), new RoslynCompiler())
    {
        SourcePreprocessors.AddRange(sourcePreprocessors);
    }

    private static ScriptClassTemplate CreateScriptClassTemplate(bool scriptObjectWithInterface,
        ITestApi testApi)
    {
        if (scriptObjectWithInterface)
        {
            return new TestScriptClassTemplate(testApi);
        }

        return new TestScriptClassWithoutInterfaceTemplate(testApi);
    }
}
