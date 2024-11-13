using System;
using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Options.Core;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Options;

[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
public class OptionsIntegrationTests
{
    private readonly ServiceCollection _services;

    public OptionsIntegrationTests()
    {
        _services = [];
        _services.AddNamedOptions<TestOptions>();
    }

    [Fact]
    public void GetOptionsSnapshot_WithNamedOptions_ReturnsOptions()
    {
        // Arrange
        const string optionsName = "test_name";
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
    public void GetOptions_WithoutNamedOptions_ReturnsDefaultOptions()
    {
        // Arrange
        const string expectedName = "Test";

        var storageProvider = A.Fake<IOptionsStorageProvider<TestOptions>>();

        A.CallTo(() => storageProvider.Read(string.Empty, A<TestOptions>.Ignored))
            .Invokes(call => call.GetArgument<TestOptions>(1)!.Name = expectedName);

        _services.AddSingleton(storageProvider);

        var serviceProvider = _services.BuildServiceProvider();

        var options = serviceProvider.GetRequiredService<IOptions<TestOptions>>();

        // Act
        var testOptions = options.Value;

        // Assert
        testOptions.Name
            .Should()
            .Be(expectedName);
    }
}
