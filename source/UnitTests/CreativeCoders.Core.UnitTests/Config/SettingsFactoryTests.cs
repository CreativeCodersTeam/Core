using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Config;
using CreativeCoders.Config.Base;
using FakeItEasy;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Config;

public class SettingsFactoryTests
{
    [Fact]
    public void CtorTest()
    {
        Assert.Throws<ArgumentNullException>(() => new SettingsFactory<object>(null));

        var config = A.Fake<IConfiguration>();
        var factory = new SettingsFactory<object>(config);

        Assert.NotNull(factory);
    }

    [Fact]
    public void CreateTest()
    {
        _objectsCreated = 0;

        var config = A.Fake<IConfiguration>();
        A.CallTo(() => config.GetItems<object>())
            .Returns(CreateNewObjects())
            .NumberOfTimes(1)
            .Then
            .Returns(CreateNewObjects());

        var factory = new SettingsFactory<object>(config);


        var items0 = factory.Create().ToArray();
        var items1 = factory.Create().ToArray();

        Assert.Equal(2, _objectsCreated);
        Assert.Single((IEnumerable) items0);
        Assert.Single(items1);
        Assert.Contains("c", items0);
        Assert.Contains("cc", items1);
        Assert.NotSame(items0, items1);
    }

    private int _objectsCreated;

    private IEnumerable<object> CreateNewObjects()
    {
        _objectsCreated++;
        return new[] {new string('c', _objectsCreated)};
    }
}