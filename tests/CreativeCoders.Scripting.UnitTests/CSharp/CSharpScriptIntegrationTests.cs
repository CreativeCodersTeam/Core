using System;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.CodeCompilation;
using CreativeCoders.Core.SysEnvironment;
using CreativeCoders.Scripting.Base;
using CreativeCoders.Scripting.Base.SourceCode;
using CreativeCoders.Scripting.CSharp;
using CreativeCoders.Scripting.CSharp.Exceptions;
using CreativeCoders.Scripting.CSharp.Preprocessors;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Scripting.UnitTests.CSharp;

public class CSharpScriptIntegrationTests
{
    [Fact]
    public void CreateAction_TestApiInjectedViaClassTemplate_ExecutionSucceeded()
    {
        var scriptSourceCode =
            "using CreativeCoders.Scripting.UnitTests.CSharp;" + Env.NewLine +
            "public void Execute() { Api.DoSomething(\"Method executed\"); }";

        var testApi = A.Fake<ITestApi>();

        var script = CreateScript(scriptSourceCode, new[] {new UsingsPreprocessor()}, testApi, false);

        var action = script.CreateAction("Execute", null);

        action();

        A.CallTo(() => testApi.DoSomething("Method executed")).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void CreateAction_TestApiInjectedViaContext_ExecutionSucceeded()
    {
        var scriptSourceCode =
            "using CreativeCoders.Scripting.UnitTests.CSharp;" + Env.NewLine +
            "public void Execute() { Api.DoSomething(\"Method executed\"); }";

        var testApi = A.Fake<ITestApi>();
        var contextTestApi = A.Fake<ITestApi>();

        var script = CreateScript(scriptSourceCode, new[] {new UsingsPreprocessor()}, testApi, false);

        var scriptContext = new ScriptContext();
        scriptContext.AddInjection(new ScriptPropertyInjection<ITestApi>("Api", () => contextTestApi));

        var action = script.CreateAction("Execute", scriptContext);

        action();

        A.CallTo(() => testApi.DoSomething("Method executed")).MustNotHaveHappened();
        A.CallTo(() => contextTestApi.DoSomething("Method executed")).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void CreateAction_ForGeneratedMethod_ApiIsCalled()
    {
        var scriptSourceCode =
            "using CreativeCoders.Scripting.UnitTests.CSharp;" + Env.NewLine +
            "public void Execute() { Api.DoSomething(\"Method executed\"); }";

        var testApi = A.Fake<ITestApi>();

        var script = CreateScript(scriptSourceCode, new[] {new UsingsPreprocessor()}, testApi, false);

        var action = script.CreateAction("CallApi", null);

        action();

        A.CallTo(() => testApi.DoSomething("Call")).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void CreateObject_ForGeneratedProperty_ApiIsCalled()
    {
        var scriptSourceCode =
            "using CreativeCoders.Scripting.UnitTests.CSharp;" + Env.NewLine +
            "public void Execute() { Api.DoSomething(\"Method executed\"); }";

        var testApi = A.Fake<ITestApi>();

        var script = CreateScript(scriptSourceCode, new[] {new UsingsPreprocessor()}, testApi, true);

        var scriptObject = script.CreateObject<ITextScript>();

        var result = scriptObject.TestText;

        Assert.Equal("SomeText", result);

        scriptObject.TestText = "MoreText";

        A.CallTo(() => testApi.DoSomething("MoreText")).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void CreateObject_ForGeneratedProperty_GetReturnsValue()
    {
        var scriptSourceCode =
            "using CreativeCoders.Scripting.UnitTests.CSharp;" + Env.NewLine +
            "public void Execute() { Api.DoSomething(\"Method executed\"); }";

        var testApi = A.Fake<ITestApi>();

        var script = CreateScript(scriptSourceCode, new[] {new UsingsPreprocessor()}, testApi, true);

        var scriptObject = script.CreateObject<ITextScript>();

        var result = scriptObject.IntValue;

        Assert.Equal(12345, result);
    }

    [Fact]
    public void CSharpScriptMethodNames_Get_FilledWithMethods()
    {
        var scriptSourceCode =
            "using CreativeCoders.Scripting.UnitTests.CSharp;" + Env.NewLine +
            "public void Execute() { Api.DoSomething(\"Method executed\"); }";

        var testApi = A.Fake<ITestApi>();

        var script = CreateScript(scriptSourceCode, new[] {new UsingsPreprocessor()}, testApi, true);

        Assert.Equal(2, script.MethodNames.Count);
        Assert.Contains(script.MethodNames, x => x == "Execute");
        Assert.Contains(script.MethodNames, x => x == "CallApi");
    }

    [Fact]
    public void Build_IncorrectSyntax_ThrowsException()
    {
        var scriptSourceCode =
            "using CreativeCoders.Scripting.UnitTests.CSharp;" + Env.NewLine +
            "public void Exe cute() { Api.DoSomething(\"Method executed\"); }";

        var testApi = A.Fake<ITestApi>();

        var scriptPackage =
            new ScriptPackage("Script0", "TestScript", new StringSourceCode(scriptSourceCode));

        var runtime =
            new CSharpScriptRuntime<TestScriptImplementation>(
                    new TestScriptImplementation(Array.Empty<ISourcePreprocessor>(), testApi, false)) as
                IScriptRuntime;

        var runtimeSpace = runtime.CreateSpace("CreativeCoders.TestScripts");

        var exception =
            Assert.Throws<ScriptCompilationFailedException>(() => runtimeSpace.Build(scriptPackage));

        Assert.Same(scriptPackage, exception.ScriptPackage);
        Assert.Contains(exception.CompilationResultMessages, x => x.MessageType == CompilationMessageType.Error);
    }

    private static IScript CreateScript(string sourceCode,
        IEnumerable<ISourcePreprocessor> sourcePreprocessors,
        ITestApi testApi, bool scriptObjectWithInterface)
    {
        var scriptPackage = new ScriptPackage("Script0", "TestScript", new StringSourceCode(sourceCode));

        var runtime =
            new CSharpScriptRuntime<TestScriptImplementation>(
                    new TestScriptImplementation(sourcePreprocessors, testApi, scriptObjectWithInterface)) as
                IScriptRuntime;

        var runtimeSpace = runtime.CreateSpace("CreativeCoders.TestScripts");

        var script = runtimeSpace.Build(scriptPackage);

        return script;
    }
}
