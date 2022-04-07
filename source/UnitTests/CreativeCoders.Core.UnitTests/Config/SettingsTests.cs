using CreativeCoders.Config;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Config;

public class SettingsTests
{
    [Fact]
    public void CtorTest()
    {
        new SettingTestHelper().TestSettingsCtor<object>(factory => new Settings<object>(factory));
    }

    [Fact]
    public void ValuesTest()
    {
        new SettingTestHelper().TestSettingsValues(factory => new Settings<object>(factory),
            () => new[] {new object(), new object()});
    }
}
