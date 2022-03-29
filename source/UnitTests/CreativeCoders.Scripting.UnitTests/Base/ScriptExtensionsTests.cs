using System;
using CreativeCoders.Scripting.Base;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Scripting.UnitTests.Base;

public class ScriptExtensionsTests
{
    [Fact]
    public void CreateAction_NoArguments_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Add"});

        var action = script.CreateAction();

        action();

        A.CallTo(() => scriptObject.Add()).MustHaveHappenedOnceExactly();
    }
        
    [Fact]
    public void CreateAction_NoArgumentsNoMethods_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(Array.Empty<string>());

        Assert.Throws<MissingMethodException>(() => script.CreateAction());
    }
        
    [Fact]
    public void CreateAction_NoArgumentsMoreThanOneMethod_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new []{"Add", "Process"});

        Assert.Throws<MissingMethodException>(() => script.CreateAction());
    }
        
    [Fact]
    public void CreateAction_WithScriptContext_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Add"});

        var action = script.CreateAction(ScriptContext.Empty);

        action();

        A.CallTo(() => scriptObject.Add()).MustHaveHappenedOnceExactly();
    }
        
    [Fact]
    public void CreateAction_WithMethodName_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var action = script.CreateAction("Add");

        action();

        A.CallTo(() => scriptObject.Add()).MustHaveHappenedOnceExactly();
    }
        
    [Fact]
    public void CreateAction_WithMethodNameAndScriptContext_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var action = script.CreateAction("Add", ScriptContext.Empty);

        action();

        A.CallTo(() => scriptObject.Add()).MustHaveHappenedOnceExactly();
    }
        
        
    [Fact]
    public void CreateActionT_NoArguments_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Add"});

        var action = script.CreateAction<int>();

        action(1234);

        A.CallTo(() => scriptObject.Add(1234)).MustHaveHappenedOnceExactly();
    }
        
    [Fact]
    public void CreateActionT_NoArgumentsNoMethods_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(Array.Empty<string>());

        Assert.Throws<MissingMethodException>(() => script.CreateAction<int>());
    }
        
    [Fact]
    public void CreateActionT_NoArgumentsMoreThanOneMethod_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new []{"Add", "Process"});

        Assert.Throws<MissingMethodException>(() => script.CreateAction<int>());
    }
        
    [Fact]
    public void CreateActionT_WithScriptContext_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Add"});

        var action = script.CreateAction<int>(ScriptContext.Empty);

        action(1234);

        A.CallTo(() => scriptObject.Add(1234)).MustHaveHappenedOnceExactly();
    }
        
    [Fact]
    public void CreateActionT_WithMethodName_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var action = script.CreateAction<int>("Add");

        action(1234);

        A.CallTo(() => scriptObject.Add(1234)).MustHaveHappenedOnceExactly();
    }
        
    [Fact]
    public void CreateActionT_WithMethodNameAndScriptContext_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var action = script.CreateAction<int>("Add", ScriptContext.Empty);

        action(1234);

        A.CallTo(() => scriptObject.Add(1234)).MustHaveHappenedOnceExactly();
    }
        
        
    [Fact]
    public void CreateActionT1T2_NoArguments_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Add"});

        var action = script.CreateAction<int, string>();

        action(1234, "Test");

        A.CallTo(() => scriptObject.Add(1234, "Test")).MustHaveHappenedOnceExactly();
    }
        
    [Fact]
    public void CreateActionT1T2_NoArgumentsNoMethods_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(Array.Empty<string>());

        Assert.Throws<MissingMethodException>(() => script.CreateAction<int, string>());
    }
        
    [Fact]
    public void CreateActionT1T2_NoArgumentsMoreThanOneMethod_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new []{"Add", "Process"});

        Assert.Throws<MissingMethodException>(() => script.CreateAction<int, string>());
    }
        
    [Fact]
    public void CreateActionT1T2_WithScriptContext_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Add"});

        var action = script.CreateAction<int, string>(ScriptContext.Empty);

        action(1234, "Test");

        A.CallTo(() => scriptObject.Add(1234, "Test")).MustHaveHappenedOnceExactly();
    }
        
    [Fact]
    public void CreateActionT1T2_WithMethodName_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var action = script.CreateAction<int, string>("Add");

        action(1234, "Test");

        A.CallTo(() => scriptObject.Add(1234, "Test")).MustHaveHappenedOnceExactly();
    }
        
    [Fact]
    public void CreateActionT1T2_WithMethodNameAndScriptContext_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var action = script.CreateAction<int, string>("Add", ScriptContext.Empty);

        action(1234, "Test");

        A.CallTo(() => scriptObject.Add(1234, "Test")).MustHaveHappenedOnceExactly();
    }
        
        
    [Fact]
    public void CreateActionT1T2T3_NoArguments_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Add"});

        var action = script.CreateAction<int, string, bool>();

        action(1234, "Test", true);

        A.CallTo(() => scriptObject.Add(1234, "Test", true)).MustHaveHappenedOnceExactly();
    }
        
    [Fact]
    public void CreateActionT1T2T3_NoArgumentsNoMethods_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(Array.Empty<string>());

        Assert.Throws<MissingMethodException>(() => script.CreateAction<int, string, bool>());
    }
        
    [Fact]
    public void CreateActionT1T2T3_NoArgumentsMoreThanOneMethod_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new []{"Add", "Process"});

        Assert.Throws<MissingMethodException>(() => script.CreateAction<int, string, bool>());
    }
        
    [Fact]
    public void CreateActionT1T2T3_WithScriptContext_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Add"});

        var action = script.CreateAction<int, string, bool>(ScriptContext.Empty);

        action(1234, "Test", true);

        A.CallTo(() => scriptObject.Add(1234, "Test", true)).MustHaveHappenedOnceExactly();
    }
        
    [Fact]
    public void CreateActionT1T2T3_WithMethodName_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var action = script.CreateAction<int, string, bool>("Add");

        action(1234, "Test", true);

        A.CallTo(() => scriptObject.Add(1234, "Test", true)).MustHaveHappenedOnceExactly();
    }
        
    [Fact]
    public void CreateActionT1T2T3_WithMethodNameAndScriptContext_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var action = script.CreateAction<int, string, bool>("Add", ScriptContext.Empty);

        action(1234, "Test", true);

        A.CallTo(() => scriptObject.Add(1234, "Test", true)).MustHaveHappenedOnceExactly();
    }
        
        
    [Fact]
    public void CreateActionT1T2T3T4_NoArguments_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Add"});

        var action = script.CreateAction<int, string, bool, object>();

        var objectArgument = new object();
            
        action(1234, "Test", true, objectArgument);

        A.CallTo(() => scriptObject.Add(1234, "Test", true, objectArgument)).MustHaveHappenedOnceExactly();
    }
        
    [Fact]
    public void CreateActionT1T2T3T4_NoArgumentsNoMethods_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(Array.Empty<string>());

        Assert.Throws<MissingMethodException>(() => script.CreateAction<int, string, bool, object>());
    }
        
    [Fact]
    public void CreateActionT1T2T3T4_NoArgumentsMoreThanOneMethod_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new []{"Add", "Process"});

        Assert.Throws<MissingMethodException>(() => script.CreateAction<int, string, bool, object>());
    }
        
    [Fact]
    public void CreateActionT1T2T3T4_WithScriptContext_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Add"});

        var action = script.CreateAction<int, string, bool, object>(ScriptContext.Empty);

        var objectArgument = new object();
            
        action(1234, "Test", true, objectArgument);

        A.CallTo(() => scriptObject.Add(1234, "Test", true, objectArgument)).MustHaveHappenedOnceExactly();
    }
        
    [Fact]
    public void CreateActionT1T2T3T4_WithMethodName_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var action = script.CreateAction<int, string, bool, object>("Add");

        var objectArgument = new object();
            
        action(1234, "Test", true, objectArgument);

        A.CallTo(() => scriptObject.Add(1234, "Test", true, objectArgument)).MustHaveHappenedOnceExactly();
    }
        
    [Fact]
    public void CreateActionT1T2T3T4_WithMethodNameAndScriptContext_ActionCallsMethod()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var action = script.CreateAction<int, string, bool, object>("Add", ScriptContext.Empty);

        var objectArgument = new object();
            
        action(1234, "Test", true, objectArgument);

        A.CallTo(() => scriptObject.Add(1234, "Test", true, objectArgument)).MustHaveHappenedOnceExactly();
    }
        
        
    [Fact]
    public void CreateFunc_NoArguments_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();

        A.CallTo(() => scriptObject.Process()).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Process"});

        var func = script.CreateFunc<int>();

        var result = func();

        A.CallTo(() => scriptObject.Process()).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
    [Fact]
    public void CreateFunc_NoArgumentsNoMethods_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process()).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(Array.Empty<string>());

        Assert.Throws<MissingMethodException>(() => script.CreateFunc<int>());
    }
        
    [Fact]
    public void CreateFunc_NoArgumentsMoreThanOneMethod_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process()).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new []{"Add", "Process"});

        Assert.Throws<MissingMethodException>(() => script.CreateFunc<int>());
    }
        
    [Fact]
    public void CreateFunc_WithScriptContext_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process()).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Process"});

        var func = script.CreateFunc<int>(ScriptContext.Empty);

        var result = func();

        A.CallTo(() => scriptObject.Process()).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
    [Fact]
    public void CreateFunc_WithMethodName_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process()).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var func = script.CreateFunc<int>("Process");

        var result = func();

        A.CallTo(() => scriptObject.Process()).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
    [Fact]
    public void CreateFunc_WithMethodNameAndScriptContext_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process()).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var func = script.CreateFunc<int>("Process", ScriptContext.Empty);

        var result = func();

        A.CallTo(() => scriptObject.Process()).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
        
    [Fact]
    public void CreateFuncT_NoArguments_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();

        A.CallTo(() => scriptObject.Process(1234)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Process"});

        var func = script.CreateFunc<int, int>();

        var result = func(1234);

        A.CallTo(() => scriptObject.Process(1234)).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
    [Fact]
    public void CreateFuncT_NoArgumentsNoMethods_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process(1234)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(Array.Empty<string>());

        Assert.Throws<MissingMethodException>(() => script.CreateFunc<int, int>());
    }
        
    [Fact]
    public void CreateFuncT_NoArgumentsMoreThanOneMethod_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process(1234)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new []{"Add", "Process"});

        Assert.Throws<MissingMethodException>(() => script.CreateFunc<int, int>());
    }
        
    [Fact]
    public void CreateFuncT_WithScriptContext_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process(1234)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Process"});

        var func = script.CreateFunc<int, int>(ScriptContext.Empty);

        var result = func(1234);

        A.CallTo(() => scriptObject.Process(1234)).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
    [Fact]
    public void CreateFuncT_WithMethodName_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process(1234)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var func = script.CreateFunc<int, int>("Process");

        var result = func(1234);

        A.CallTo(() => scriptObject.Process(1234)).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
    [Fact]
    public void CreateFuncT_WithMethodNameAndScriptContext_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process(1234)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var func = script.CreateFunc<int, int>("Process", ScriptContext.Empty);

        var result = func(1234);

        A.CallTo(() => scriptObject.Process(1234)).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
        
    [Fact]
    public void CreateFuncT1T2_NoArguments_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();

        A.CallTo(() => scriptObject.Process(1234, "Test")).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Process"});

        var func = script.CreateFunc<int, string, int>();

        var result = func(1234, "Test");

        A.CallTo(() => scriptObject.Process(1234, "Test")).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
    [Fact]
    public void CreateFuncT1T2_NoArgumentsNoMethods_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process(1234, "Test")).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(Array.Empty<string>());

        Assert.Throws<MissingMethodException>(() => script.CreateFunc<int, string, int>());
    }
        
    [Fact]
    public void CreateFuncT1T2_NoArgumentsMoreThanOneMethod_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process(1234, "Test")).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new []{"Add", "Process"});

        Assert.Throws<MissingMethodException>(() => script.CreateFunc<int, string, int>());
    }
        
    [Fact]
    public void CreateFuncT1T2_WithScriptContext_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process(1234, "Test")).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Process"});

        var func = script.CreateFunc<int, string, int>(ScriptContext.Empty);

        var result = func(1234, "Test");

        A.CallTo(() => scriptObject.Process(1234, "Test")).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
    [Fact]
    public void CreateFuncT1T2_WithMethodName_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process(1234, "Test")).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var func = script.CreateFunc<int, string, int>("Process");

        var result = func(1234, "Test");

        A.CallTo(() => scriptObject.Process(1234, "Test")).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
    [Fact]
    public void CreateFuncT1T2_WithMethodNameAndScriptContext_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process(1234, "Test")).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var func = script.CreateFunc<int, string, int>("Process", ScriptContext.Empty);

        var result = func(1234, "Test");

        A.CallTo(() => scriptObject.Process(1234, "Test")).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
        
    [Fact]
    public void CreateFuncT1T2T3_NoArguments_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();

        A.CallTo(() => scriptObject.Process(1234, "Test", true)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Process"});

        var func = script.CreateFunc<int, string, bool, int>();

        var result = func(1234, "Test", true);

        A.CallTo(() => scriptObject.Process(1234, "Test", true)).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
    [Fact]
    public void CreateFuncT1T2T3_NoArgumentsNoMethods_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process(1234, "Test", true)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(Array.Empty<string>());

        Assert.Throws<MissingMethodException>(() => script.CreateFunc<int, string, bool, int>());
    }
        
    [Fact]
    public void CreateFuncT1T2T3_NoArgumentsMoreThanOneMethod_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process(1234, "Test", true)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new []{"Add", "Process"});

        Assert.Throws<MissingMethodException>(() => script.CreateFunc<int, string, bool, int>());
    }
        
    [Fact]
    public void CreateFuncT1T2T3_WithScriptContext_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process(1234, "Test", true)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Process"});

        var func = script.CreateFunc<int, string, bool, int>(ScriptContext.Empty);

        var result = func(1234, "Test", true);

        A.CallTo(() => scriptObject.Process(1234, "Test", true)).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
    [Fact]
    public void CreateFuncT1T2T3_WithMethodName_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process(1234, "Test", true)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var func = script.CreateFunc<int, string, bool, int>("Process");

        var result = func(1234, "Test", true);

        A.CallTo(() => scriptObject.Process(1234, "Test", true)).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
    [Fact]
    public void CreateFuncT1T2T3_WithMethodNameAndScriptContext_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        A.CallTo(() => scriptObject.Process(1234, "Test", true)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var func = script.CreateFunc<int, string, bool, int>("Process", ScriptContext.Empty);

        var result = func(1234, "Test", true);

        A.CallTo(() => scriptObject.Process(1234, "Test", true)).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
        
    [Fact]
    public void CreateFuncT1T2T3T4_NoArguments_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();

        var objectArgument = new object();
            
        A.CallTo(() => scriptObject.Process(1234, "Test", true, objectArgument)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Process"});

        var func = script.CreateFunc<int, string, bool, object, int>();

        var result = func(1234, "Test", true, objectArgument);

        A.CallTo(() => scriptObject.Process(1234, "Test", true, objectArgument)).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
    [Fact]
    public void CreateFuncT1T2T3T4_NoArgumentsNoMethods_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        var objectArgument = new object();
            
        A.CallTo(() => scriptObject.Process(1234, "Test", true, objectArgument)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(Array.Empty<string>());

        Assert.Throws<MissingMethodException>(() => script.CreateFunc<int, string, bool, object, int>());
    }
        
    [Fact]
    public void CreateFuncT1T2T3T4_NoArgumentsMoreThanOneMethod_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        var objectArgument = new object();
            
        A.CallTo(() => scriptObject.Process(1234, "Test", true, objectArgument)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new []{"Add", "Process"});

        Assert.Throws<MissingMethodException>(() => script.CreateFunc<int, string, bool, object, int>());
    }
        
    [Fact]
    public void CreateFuncT1T2T3T4_WithScriptContext_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        var objectArgument = new object();
            
        A.CallTo(() => scriptObject.Process(1234, "Test", true, objectArgument)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);
        A.CallTo(() => script.MethodNames).Returns(new[] {"Process"});

        var func = script.CreateFunc<int, string, bool, object, int>(ScriptContext.Empty);

        var result = func(1234, "Test", true, objectArgument);

        A.CallTo(() => scriptObject.Process(1234, "Test", true, objectArgument)).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
    [Fact]
    public void CreateFuncT1T2T3T4_WithMethodName_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        var objectArgument = new object();
            
        A.CallTo(() => scriptObject.Process(1234, "Test", true, objectArgument)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var func = script.CreateFunc<int, string, bool, object, int>("Process");

        var result = func(1234, "Test", true, objectArgument);

        A.CallTo(() => scriptObject.Process(1234, "Test", true, objectArgument)).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
        
    [Fact]
    public void CreateFuncT1T2T3T4_WithMethodNameAndScriptContext_FuncCallsMethodAndReturnsResult()
    {
        var scriptObject = A.Fake<IScriptObject>();
        var script = A.Fake<IScript>();
            
        var objectArgument = new object();
            
        A.CallTo(() => scriptObject.Process(1234, "Test", true, objectArgument)).Returns(3456);
        A.CallTo(() => script.CreateObject<object>(ScriptContext.Empty)).Returns(scriptObject);

        var func = script.CreateFunc<int, string, bool, object, int>("Process", ScriptContext.Empty);

        var result = func(1234, "Test", true, objectArgument);

        A.CallTo(() => scriptObject.Process(1234, "Test", true, objectArgument)).MustHaveHappenedOnceExactly();
        Assert.Equal(3456, result);
    }
}