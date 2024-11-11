using CreativeCoders.Options.Core;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Options;

public class OptionsIntegrationTests
{
    private readonly ServiceCollection _services;

    public OptionsIntegrationTests()
    {
        _services = new ServiceCollection();
        _services.AddNamedOptions<TestOptions>();
    }

    [Fact]
    public void GetRequiredService_WithNamedOptions_ReturnsOptions()
    {
        // Arrange
        const string optionsName = "testname";
        const string expectedName = "Test";

        var storageProvider = A.Fake<IOptionsStorageProvider<TestOptions>>();

        A.CallTo(() => storageProvider.Read(optionsName, A<TestOptions>.Ignored))
            .Invokes(call => call.GetArgument<TestOptions>(1)!.Name = expectedName);

        _services.AddSingleton(storageProvider);

        var serviceProvider = _services.BuildServiceProvider();

        var optionsSnapshots = serviceProvider.GetRequiredService<IOptionsSnapshot<TestOptions>>();

        // Act
        var options = optionsSnapshots.Get(optionsName);

        // Assert
        options.Name
            .Should()
            .Be(expectedName);
    }

    [Fact]
    public void GetRequiredService_WithoutNamedOptions_ReturnsDefaultOptions()
    {
        // Arrange
        //const string optionsName = "testname";
        const string expectedName = "Test";

        var storageProvider = A.Fake<IOptionsStorageProvider<TestOptions>>();

        A.CallTo(() => storageProvider.Read(string.Empty, A<TestOptions>.Ignored))
            .Invokes(call => call.GetArgument<TestOptions>(1)!.Name = expectedName);

        _services.AddSingleton(storageProvider);

        var serviceProvider = _services.BuildServiceProvider();

        var optionsSnapshots = serviceProvider.GetRequiredService<IOptions<TestOptions>>();

        // Act
        var options = optionsSnapshots.Value;

        // Assert
        options.Name
            .Should()
            .Be(expectedName);
    }
}
