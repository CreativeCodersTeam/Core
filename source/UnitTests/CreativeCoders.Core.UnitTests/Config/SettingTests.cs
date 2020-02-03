using CreativeCoders.Config;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Config
{
    public class SettingTests
    {
        [Fact]
        public void CtorTest()
        {
            new SettingTestHelper().TestSettingCtor<object>(factory => new Setting<object>(factory));
        }

        [Fact]
        public void ValueTest()
        {
            new SettingTestHelper().TestSettingValue(factory => new Setting<object>(factory), () => new object());
        }
    }
}