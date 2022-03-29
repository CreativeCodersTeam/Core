using System;
using System.IO;
using System.Linq;
using CreativeCoders.Config;
using CreativeCoders.Config.Base;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Config;

public class ConfigurationTests
{
    [Fact]
    public void CtorTest()
    {
        // ReSharper disable once ObjectCreationAsStatement
        new Configuration();
    }

    [Fact]
    public void AddSourceTest()
    {
        var config = new Configuration();

        var source = A.Fake<IConfigurationSource<object>>();

        Assert.Throws<ArgumentNullException>(() => config.AddSource<object>(null));

        config.AddSource(source);
    }

    [Fact]
    public void AddSourcesTest()
    {
        var config = new Configuration();

        var source0 = A.Fake<IConfigurationSource<object>>();
        var source1 = A.Fake<IConfigurationSource<object>>();

        Assert.Throws<ArgumentNullException>(() => config.AddSources<object>(null));

        config.AddSources(new []{source0, source1});
    }

    [Fact]
    public void GetItemTest()
    {
        var config = new Configuration();

        var settingObject = new object();
        var source = A.Fake<IConfigurationSource<object>>();
        A.CallTo(() => source.GetSettingObject()).Returns(settingObject);

        config.AddSource(source);

        var obj = config.GetItem<object>();

        Assert.Equal(settingObject, obj);
    }

    [Fact]
    public void GetItemTestThrowsException()
    {
        var config = new Configuration();

        var _ = new object();
        var source = A.Fake<IConfigurationSource<object>>();
        A.CallTo(() => source.GetSettingObject()).Throws(_ => new FileNotFoundException());

        config.AddSource(source);

        Assert.Throws<FileNotFoundException>(() => config.GetItem<object>());            
    }

    [Fact]
    public void GetItemTestExceptionActionGetsCalled()
    {
        var config = new Configuration();

        var settingObject = new object();
        var source = A.Fake<IConfigurationSource<object>>();
        A.CallTo(() => source.GetSettingObject()).Throws(_ => new FileNotFoundException());
        A.CallTo(() => source.GetDefaultSettingObject()).Returns(settingObject);

        config.AddSource(source);
        var exceptionActionCalled = false;
        config.OnSourceException((_, _, handleResult) =>
        {
            handleResult.IsHandled = true;
            exceptionActionCalled = true;
        });

        var item = config.GetItem<object>();

        Assert.True(exceptionActionCalled);
        Assert.Same(settingObject, item);
    }

    [Fact]
    public void GetItemTestGetDefaultSetting()
    {
        var config = new Configuration();

        var settingObject = new object();
        var source = A.Fake<IConfigurationSource<object>>();
        A.CallTo(() => source.GetSettingObject()).Throws(_ => new FileNotFoundException());
        A.CallTo(() => source.GetDefaultSettingObject()).Returns(settingObject);

        config.AddSource(source);
        config.OnSourceException((_, _, handleResult) => handleResult.IsHandled = true);

        var item = config.GetItem<object>();

        Assert.Same(settingObject, item);            
    }

    [Fact]
    public void GetItemsTest()
    {
        var config = new Configuration();

        var settingObject0 = new object();
        var settingObject1 = new object();

        var source0 = A.Fake<IConfigurationSource<object>>();
        var source1 = A.Fake<IConfigurationSource<object>>();

        A.CallTo(() => source0.GetSettingObject()).Returns(settingObject0);
        A.CallTo(() => source1.GetSettingObject()).Returns(settingObject1);

        config.AddSources(new[] {source0, source1});

        var items = config.GetItems<object>().ToArray();

        Assert.Equal(2, items.Length);
        Assert.Contains(settingObject0, items);
        Assert.Contains(settingObject1, items);
    }
}