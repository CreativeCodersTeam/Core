using CreativeCoders.Config;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Config
{
    public class SettingTransientTests
    {
        [Fact]
        public void CtorTest()
        {
            new SettingTestHelper().TestSettingCtor<object>(factory => new SettingTransient<object>(factory));
        }

        [Fact]
        public void ValueTest()
        {
            new SettingTestHelper().TestSettingValue(factory => new SettingTransient<object>(factory), () => new object());
        }
    }
}