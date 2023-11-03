using System;
using CreativeCoders.Config;
using CreativeCoders.Config.Base;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Config;

public class SettingFactoryTests
{
    [Fact]
    public void CtorTest()
    {
        Assert.Throws<ArgumentNullException>(() => new SettingFactory<object>(null));

        var config = A.Fake<IConfiguration>();
        var factory = new SettingFactory<object>(config);

        Assert.NotNull(factory);
    }

    [Fact]
    public void CreateTest()
    {
        _objectsCreated = 0;

        var config = A.Fake<IConfiguration>();
        A.CallTo(() => config.GetItem<object>())
            .Returns(CreateNewObject())
            .NumberOfTimes(1)
            .Then
            .Returns(CreateNewObject());

        var factory = new SettingFactory<object>(config);

        var obj0 = factory.Create();
        var obj1 = factory.Create();

        Assert.Equal(2, _objectsCreated);
        Assert.Equal("c", obj0);
        Assert.Equal("cc", obj1);
        Assert.NotSame(obj0, obj1);
    }

    private int _objectsCreated;

    private object CreateNewObject()
    {
        _objectsCreated++;
        return new string('c', _objectsCreated);
    }
}
