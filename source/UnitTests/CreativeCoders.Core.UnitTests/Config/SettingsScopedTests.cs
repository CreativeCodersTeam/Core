using CreativeCoders.Config;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Config;

public class SettingsScopedTests
{
    [Fact]
    public void CtorTest()
    {
        new SettingTestHelper().TestSettingsCtor<object>(factory => new SettingsScoped<object>(factory));
    }

    [Fact]
    public void ValuesTest()
    {
        new SettingTestHelper().TestSettingsValues(factory => new SettingsScoped<object>(factory),
            () => new[] { new object(), new object() });
    }
}