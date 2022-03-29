using System;
using CreativeCoders.Config.Sources;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Config.Sources;

public class ConfigurationSourceTests
{
    [Fact]
    public void CtorTest()
    {
        Assert.Throws<ArgumentNullException>(() => new ConfigurationSource<object>(null));
        Assert.Throws<ArgumentNullException>(() => new ConfigurationSource<object>(() => new object(), null));

        var obj = new object();

        var source = new ConfigurationSource<object>(() => obj);

        Assert.NotNull(source);
    }

    [Fact]
    public void GetSettingObjectTest()
    {
        var obj = new object();

        var source = new ConfigurationSource<object>(() => obj);

        var settingObject = source.GetSettingObject();

        Assert.Same(obj, settingObject);
    }

    [Fact]
    public void GetDefaultSettingObjectTest()
    {
        var obj = new object();
        var defaultObj = new object();

        var source = new ConfigurationSource<object>(() => obj, () => defaultObj);

        var defaultSettingObject = source.GetDefaultSettingObject();
        var settingObject = source.GetSettingObject();

        Assert.Same(defaultObj, defaultSettingObject);
        Assert.Same(obj, settingObject);
    }

    [Fact]
    public void GetDefaultSettingObjectTestWithDefaultCtor()
    {
        var obj = new TestSetting {Text = "Test"};
            
        var source = new ConfigurationSource<TestSetting>(() => obj);

        var settingObject = source.GetDefaultSettingObject() as TestSetting;

        Assert.NotNull(settingObject);
        Assert.Equal("DefaultCtorText", settingObject.Text);
    }

    private class TestSetting
    {
        public TestSetting()
        {
            Text = "DefaultCtorText";
        }

        public string Text { get; init; }
    }
}