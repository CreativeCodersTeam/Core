using CreativeCoders.Config;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Config;

public class SettingScopedTests
{
    [Fact]
    public void CtorTest()
    {
        new SettingTestHelper().TestSettingCtor<object>(factory => new SettingScoped<object>(factory));
    }

    [Fact]
    public void ValueTest()
    {
        new SettingTestHelper().TestSettingValue(factory => new SettingScoped<object>(factory), () => new object());
    }
}