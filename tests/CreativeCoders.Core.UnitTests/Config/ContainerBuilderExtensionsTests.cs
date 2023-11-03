using System;
using System.Linq;
using CreativeCoders.Config;
using CreativeCoders.Config.Base;
using CreativeCoders.Config.Sources;
using FakeItEasy;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Config;

public class ContainerBuilderExtensionsTests
{
    [Fact]
    public void ConfigureTestConfiguration()
    {
        var services = new ServiceCollection();

        var config = A.Fake<IConfiguration>();

        services.AddConfigSystem(config);

        var serviceProvider = services.BuildServiceProvider();

        var config0 = serviceProvider.GetService<IConfiguration>();
        var config1 = serviceProvider.GetService<IConfiguration>();

        Assert.Same(config, config0);
        Assert.Same(config, config1);
    }

    [Fact]
    public void ConfigureTestSetting()
    {
        var services = new ServiceCollection();

        var config = new Configuration();
        config.AddSource(new ConfigurationSource<DemoSetting>(() => new DemoSetting {Text = "DemoValue"}));

        services.AddConfigSystem(config);

        var serviceProvider = services.BuildServiceProvider();

        var setting0 = serviceProvider.GetRequiredService<ISetting<DemoSetting>>();
        var setting1 = serviceProvider.GetRequiredService<ISetting<DemoSetting>>();

        Assert.Equal("DemoValue", setting0.Value.Text);
        Assert.Equal("DemoValue", setting1.Value.Text);
        Assert.Same(setting0, setting1);
        Assert.Same(setting0.Value, setting1.Value);
    }

    [Fact]
    public void ConfigureTestSettingTransient()
    {
        var services = new ServiceCollection();

        var config = new Configuration();
        config.AddSource(new ConfigurationSource<DemoSetting>(() => new DemoSetting {Text = "DemoValue"}));

        services.AddConfigSystem(config);

        var serviceProvider = services.BuildServiceProvider();

        var setting0 = serviceProvider.GetRequiredService<ISettingTransient<DemoSetting>>();
        var setting1 = serviceProvider.GetRequiredService<ISettingTransient<DemoSetting>>();

        Assert.Equal("DemoValue", setting0.Value.Text);
        Assert.Equal("DemoValue", setting1.Value.Text);
        Assert.NotSame(setting0, setting1);
        Assert.NotSame(setting0.Value, setting1.Value);
    }

    [Fact]
    public void ConfigureTestSettingScoped()
    {
        var services = new ServiceCollection();

        var config = new Configuration();
        config.AddSource(new ConfigurationSource<DemoSetting>(() => new DemoSetting {Text = "DemoValue"}));

        services.AddConfigSystem(config);

        var serviceProvider = services.BuildServiceProvider();

        var scope = serviceProvider.CreateScope();
        var setting0 = scope.ServiceProvider.GetRequiredService<ISettingScoped<DemoSetting>>();
        var setting1 = scope.ServiceProvider.GetRequiredService<ISettingScoped<DemoSetting>>();
        scope.Dispose();

        var scope1 = serviceProvider.CreateScope();
        var setting2 = scope1.ServiceProvider.GetRequiredService<ISettingScoped<DemoSetting>>();
        var setting3 = scope1.ServiceProvider.GetRequiredService<ISettingScoped<DemoSetting>>();
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
        var services = new ServiceCollection();

        var config = new Configuration();
        config.AddSource(new ConfigurationSource<DemoSetting>(() => new DemoSetting {Text = "DemoValue"}));

        services.AddConfigSystem(config);

        var serviceProvider = services.BuildServiceProvider();

        var settings0 = serviceProvider.GetRequiredService<ISettings<DemoSetting>>();
        var settings1 = serviceProvider.GetRequiredService<ISettings<DemoSetting>>();

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
        var services = new ServiceCollection();

        var config = new Configuration();
        config.AddSource(new ConfigurationSource<DemoSetting>(() => new DemoSetting {Text = "DemoValue"}));

        services.AddConfigSystem(config);

        var serviceProvider = services.BuildServiceProvider();

        var settings0 = serviceProvider.GetRequiredService<ISettingsTransient<DemoSetting>>();
        var settings1 = serviceProvider.GetRequiredService<ISettingsTransient<DemoSetting>>();

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
        var services = new ServiceCollection();

        var config = new Configuration();
        config.AddSource(new ConfigurationSource<DemoSetting>(() => new DemoSetting {Text = "DemoValue"}));

        services.AddConfigSystem(config);

        var container = services.BuildServiceProvider();

        var scope0 = container.CreateScope();
        var settings0 = scope0.ServiceProvider.GetRequiredService<ISettingsScoped<DemoSetting>>();
        var settings1 = scope0.ServiceProvider.GetRequiredService<ISettingsScoped<DemoSetting>>();
        scope0.Dispose();

        var scope1 = container.CreateScope();
        var settings2 = scope1.ServiceProvider.GetRequiredService<ISettingsScoped<DemoSetting>>();
        var settings3 = scope1.ServiceProvider.GetRequiredService<ISettingsScoped<DemoSetting>>();
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
