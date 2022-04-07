using System;
using CreativeCoders.Config.Sources.Json;
using Xunit;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using CreativeCoders.Config.Base.Exceptions;
using CreativeCoders.UnitTests;

namespace CreativeCoders.Core.UnitTests.Config.Sources;

[Collection("FileSys")]
public class JsonConfigurationSourceTests
{
    [Fact]
    public void CtorTest()
    {
        Assert.Throws<ArgumentException>(() => new JsonConfigurationSource<object>(null));
        Assert.Throws<ArgumentException>(() => new JsonConfigurationSource<object>(string.Empty));
        Assert.Throws<ArgumentException>(() => new JsonConfigurationSource<object>("  "));

        var source = new JsonConfigurationSource<object>("data.config");

        Assert.NotNull(source);
    }

    private static void InstallDemoFileSys()
    {
        var fileSystem = new MockFileSystemEx();
        fileSystem.AddFile(@"c:\temp\data.config",
            new MockFileData("{\"Text\":\"DemoDataValue\"}"));

        fileSystem.Install();
    }

    [Fact]
    public void GetSettingObjectTest()
    {
        InstallDemoFileSys();

        var source = new JsonConfigurationSource<DemoSetting>(@"c:\temp\data.config");

        var settingObject = source.GetSettingObject() as DemoSetting;

        Assert.Equal("DemoDataValue", settingObject?.Text);
    }

    [Fact]
    public void GetDefaultSettingObjectTest()
    {
        InstallDemoFileSys();

        var defaultObj = new DemoSetting {Text = "Test"};

        var source = new JsonConfigurationSource<DemoSetting>(@"c:\temp\data.config", () => defaultObj);

        var defaultSettingObject = source.GetDefaultSettingObject() as DemoSetting;
        var settingObject = source.GetSettingObject() as DemoSetting;

        Assert.Equal("Test", defaultSettingObject?.Text);
        Assert.Equal("DemoDataValue", settingObject?.Text);
    }

    [Fact]
    public void GetDefaultSettingObjectTestWithDefaultCtor()
    {
        InstallDemoFileSys();

        var source = new JsonConfigurationSource<DemoSetting>(@"c:\temp\data.config");

        var defaultSettingObject = source.GetDefaultSettingObject() as DemoSetting;
        var settingObject = source.GetSettingObject() as DemoSetting;

        Assert.Equal("DemoDataValue", settingObject?.Text);
        Assert.NotNull(defaultSettingObject);
        Assert.Null(defaultSettingObject.Text);
    }

    [Fact]
    public void GetSettingObjectTestThrowException()
    {
        var source = new JsonConfigurationSource<DemoSetting>(@"c:\temp\data1234.config");

        var ex = Assert.Throws<ConfigurationFileSourceException>(() => source.GetSettingObject());

        Assert.Equal(@"c:\temp\data1234.config", ex.FileName);
        Assert.Same(source, ex.ConfigurationSource);
    }

    [Fact]
    public void FromFilesTest()
    {
        var fileSystem = new MockFileSystemEx();
        fileSystem.AddFile(@"c:\temp\data0.config", new MockFileData("{\"Text\":\"DemoDataValue0\"}"));
        fileSystem.AddFile(@"c:\temp\data1.config", new MockFileData("{\"Text\":\"DemoDataValue1\"}"));

        fileSystem.Install();

        var sources =
            JsonConfigurationSource<DemoSetting>.FromFiles(new[]
                {@"c:\temp\data0.config", @"c:\temp\data1.config"}).ToArray();

        Assert.Equal(2, sources.Length);
        Assert.Contains("DemoDataValue0",
            sources.Select(source => (source.GetSettingObject() as DemoSetting)?.Text));
        Assert.Contains("DemoDataValue1",
            sources.Select(source => (source.GetSettingObject() as DemoSetting)?.Text));
    }

    [Fact]
    public void FromFilesTestDefaultSettings()
    {
        var fileSystem = new MockFileSystemEx();
        fileSystem.AddFile(@"c:\temp\data0.config", new MockFileData("{\"Text\":\"DemoDataValue0\"}"));
        fileSystem.AddFile(@"c:\temp\data1.config", new MockFileData("{\"Text\":\"DemoDataValue1\"}"));

        fileSystem.Install();

        var sources =
            JsonConfigurationSource<DemoSetting>.FromFiles(new[]
                        {@"c:\temp\data10.config", @"c:\temp\data21.config"},
                    () => new DemoSetting {Text = "DemoData"})
                .ToArray();

        Assert.Equal(2, sources.Length);

        Assert.Contains("DemoData",
            sources.Select(source => (source.GetDefaultSettingObject() as DemoSetting)?.Text));
        Assert.Contains("DemoData",
            sources.Select(source => (source.GetDefaultSettingObject() as DemoSetting)?.Text));
    }

    private class DemoSetting
    {
        public string Text { get; init; }
    }
}
