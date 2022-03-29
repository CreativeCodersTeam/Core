using CreativeCoders.Scripting.Base;
using CreativeCoders.Scripting.Base.Exceptions;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Scripting.UnitTests.Base;

public class ScriptPropertyInjectionTests
{
    [Fact]
    public void Inject_PropertyNotExists_NothingHappened()
    {
        var scriptObject = A.Fake<IScriptObject>();

        var propertyInjection =
            new ScriptPropertyInjection<int>(nameof(IScriptObject.IntValue) + "1", () => 1234);

        propertyInjection.Inject(scriptObject);

        A.CallTo(() => scriptObject.IntValue).MustNotHaveHappened();
    }

    [Fact]
    public void Inject_PropertyNotExists_ThrowsException()
    {
        var scriptObject = A.Fake<IScriptObject>();

        var propertyInjection =
            new ScriptPropertyInjection<int>(nameof(IScriptObject.IntValue) + "1", () => 1234, true);

        Assert.Throws<InjectionFailedException>(() => propertyInjection.Inject(scriptObject));
    }

    [Fact]
    public void Inject_PropertyExists_PropertyValueIsSet()
    {
        var scriptObject = A.Fake<IScriptObject>();

        var propertyInjection = new ScriptPropertyInjection<int>(nameof(IScriptObject.IntValue), () => 1234);

        propertyInjection.Inject(scriptObject);

        A.CallToSet(() => scriptObject.IntValue).MustHaveHappenedOnceExactly();

        Assert.Equal(1234, scriptObject.IntValue);
    }
}
