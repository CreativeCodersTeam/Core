using System;
using System.Linq;
using CreativeCoders.Config;
using CreativeCoders.Config.Base;
using CreativeCoders.Config.Sources;
using CreativeCoders.Di.Building;
using CreativeCoders.Di.SimpleInjector;
using FakeItEasy;
using JetBrains.Annotations;
using SimpleInjector;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Config;

public class ContainerBuilderExtensionsTests
{
    [Fact]
    public void ConfigureTest()
    {
        var builder = A.Fake<IDiContainerBuilder>();

        Assert.Throws<ArgumentNullException>(() => builder.Configure(null));

        var config = A.Fake<IConfiguration>();

        builder.Configure(config);
    }

    [Fact]
    public void ConfigureTestConfiguration()
    {
        var builder = new SimpleInjectorDiContainerBuilder(new Container());

        var config = A.Fake<IConfiguration>();

        builder.Configure(config);

        var container = builder.Build();

        var config0 = container.GetInstance<IConfiguration>();
        var config1 = container.GetInstance<IConfiguration>();

        Assert.Same(config, config0);
        Assert.Same(config, config1);
    }

    [Fact]
    public void ConfigureTestSetting()
    {
        var builder = new SimpleInjectorDiContainerBuilder(new Container());

        var config = new Configuration();
        config.AddSource(new ConfigurationSource<DemoSetting>(() => new DemoSetting {Text = "DemoValue"}));

        builder.Configure(config);

        var container = builder.Build();

        var setting0 = container.GetInstance<ISetting<DemoSetting>>();
        var setting1 = container.GetInstance<ISetting<DemoSetting>>();

        Assert.Equal("DemoValue", setting0.Value.Text);
        Assert.Equal("DemoValue", setting1.Value.Text);
        Assert.Same(setting0, setting1);
        Assert.Same(setting0.Value, setting1.Value);
    }

    [Fact]
    public void ConfigureTestSettingTransient()
    {
        var builder = new SimpleInjectorDiContainerBuilder(new Container());

        var config = new Configuration();
        config.AddSource(new ConfigurationSource<DemoSetting>(() => new DemoSetting {Text = "DemoValue"}));

        builder.Configure(config);

        var container = builder.Build();

        var setting0 = container.GetInstance<ISettingTransient<DemoSetting>>();
        var setting1 = container.GetInstance<ISettingTransient<DemoSetting>>();

        Assert.Equal("DemoValue", setting0.Value.Text);
        Assert.Equal("DemoValue", setting1.Value.Text);
        Assert.NotSame(setting0, setting1);
        Assert.NotSame(setting0.Value, setting1.Value);
    }

    [Fact]
    public void ConfigureTestSettingScoped()
    {
        var builder = new SimpleInjectorDiContainerBuilder(new Container());

        var config = new Configuration();
        config.AddSource(new ConfigurationSource<DemoSetting>(() => new DemoSetting {Text = "DemoValue"}));

        builder.Configure(config);

        var container = builder.Build();

        var scope = container.CreateScope();
        var setting0 = scope.Container.GetInstance<ISettingScoped<DemoSetting>>();
        var setting1 = scope.Container.GetInstance<ISettingScoped<DemoSetting>>();
        scope.Dispose();

        var scope1 = container.CreateScope();
        var setting2 = scope1.Container.GetInstance<ISettingScoped<DemoSetting>>();
        var setting3 = scope1.Container.GetInstance<ISettingScoped<DemoSetting>>();
        scope1.Dispose();

        Assert.Equal("DemoValue", setting0.Value.Text);
        Assert.Equal("DemoValue", setting1.Value.Text);
        Assert.Same(setting0, setting1);
        Assert.Same(setting0.Value, setting1.Value);

        Assert.Equal("DemoValue", setting2.Value.Text);
        Assert.Equal("DemoValue", setting3.Value.Text);
        Assert.Same(setting2, setting3);
        Assert.Same(setting2.Value, setting3.Value);

        Assert.NotSame(setting0, setting2);
        Assert.NotSame(setting0.Value, setting2.Value);
    }

    [Fact]
    public void ConfigureTestSettings()
    {
        var builder = new SimpleInjectorDiContainerBuilder(new Container());

        var config = new Configuration();
        config.AddSource(new ConfigurationSource<DemoSetting>(() => new DemoSetting {Text = "DemoValue"}));

        builder.Configure(config);

        var container = builder.Build();

        var settings0 = container.GetInstance<ISettings<DemoSetting>>();
        var settings1 = container.GetInstance<ISettings<DemoSetting>>();

        Assert.Single(settings0.Values);
        Assert.Single(settings1.Values);

        Assert.Contains("DemoValue", settings0.Values.Select(value => value.Text));
        Assert.Contains("DemoValue", settings1.Values.Select(value => value.Text));

        Assert.Same(settings0, settings1);

        var value0 = settings0.Values.First();
        var value1 = settings1.Values.First();

        Assert.Same(value0, value1);
    }

    [Fact]
    public void ConfigureTestSettingsTransient()
    {
        var builder = new SimpleInjectorDiContainerBuilder(new Container());

        var config = new Configuration();
        config.AddSource(new ConfigurationSource<DemoSetting>(() => new DemoSetting {Text = "DemoValue"}));

        builder.Configure(config);

        var container = builder.Build();

        var settings0 = container.GetInstance<ISettingsTransient<DemoSetting>>();
        var settings1 = container.GetInstance<ISettingsTransient<DemoSetting>>();

        Assert.Single(settings0.Values);
        Assert.Single(settings1.Values);

        Assert.Contains("DemoValue", settings0.Values.Select(value => value.Text));
        Assert.Contains("DemoValue", settings1.Values.Select(value => value.Text));

        Assert.NotSame(settings0, settings1);

        var value0 = settings0.Values.First();
        var value1 = settings1.Values.First();

        Assert.NotSame(value0, value1);
    }

    [Fact]
    public void ConfigureTestSettingsScoped()
    {
        var builder = new SimpleInjectorDiContainerBuilder(new Container());

        var config = new Configuration();
        config.AddSource(new ConfigurationSource<DemoSetting>(() => new DemoSetting {Text = "DemoValue"}));

        builder.Configure(config);

        var container = builder.Build();

        var scope0 = container.CreateScope();
        var settings0 = scope0.Container.GetInstance<ISettingsScoped<DemoSetting>>();
        var settings1 = scope0.Container.GetInstance<ISettingsScoped<DemoSetting>>();
        scope0.Dispose();

        var scope1 = container.CreateScope();
        var settings2 = scope0.Container.GetInstance<ISettingsScoped<DemoSetting>>();
        var settings3 = scope0.Container.GetInstance<ISettingsScoped<DemoSetting>>();
        scope1.Dispose();

        Assert.Single(settings0.Values);
        Assert.Single(settings1.Values);

        Assert.Contains("DemoValue", settings0.Values.Select(value => value.Text));
        Assert.Contains("DemoValue", settings1.Values.Select(value => value.Text));

        Assert.Same(settings0, settings1);

        var value0 = settings0.Values.First();
        var value1 = settings1.Values.First();

        Assert.Same(value0, value1);

        Assert.NotSame(settings0, settings2);

        var value2 = settings2.Values.First();
        var value3 = settings3.Values.First();

        Assert.Same(value2, value3);
        Assert.NotSame(value0, value2);
    }

    [PublicAPI]
    public class DemoSetting
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string Text { get; set; }
    }
}
