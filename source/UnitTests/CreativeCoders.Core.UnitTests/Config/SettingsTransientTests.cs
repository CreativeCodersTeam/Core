using CreativeCoders.Config;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Config
{
    public class SettingsTransientTests
    {
        [Fact]
        public void CtorTest()
        {
            new SettingTestHelper().TestSettingsCtor<object>(factory => new SettingsTransient<object>(factory));
        }

        [Fact]
        public void ValuesTest()
        {
            new SettingTestHelper().TestSettingsValues(factory => new SettingsTransient<object>(factory),
                () => new[] { new object(), new object() });
        }
    }
}