using System;
using CreativeCoders.Core.IO;
using CreativeCoders.Options.Core;
using CreativeCoders.Options.Serializers;
using CreativeCoders.Options.Storage.FileSystem;
using CreativeCoders.UnitTests;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Options;

[Collection("FileSys")]
public class OptionsWithFileSystemStorageIntegrationTests
{
    private readonly MockFileSystemEx _fileSystem;

    private readonly ServiceProvider _sp;

    public OptionsWithFileSystemStorageIntegrationTests()
    {
        var services = new ServiceCollection();

        _fileSystem = new MockFileSystemEx();
        _fileSystem.Install();

        services.AddFileSystemOptionsStorage<TestOptions>("/temp");

        services.AddOptionsYamlSerializer<TestOptions>();

        _sp = services.BuildServiceProvider();
    }

    [Fact]
    public void GetOptionsFromFileSystem_ViaIOptionsSnapshot_ReturnsOptions()
    {
        // Arrange
        const string optionsName = "test_name";
        const string expectedName = "Test";

        var yaml =
            $"Name: Test{Environment.NewLine}IntValue: 42{Environment.NewLine}IsEnabled: true{Environment.NewLine}";

        _fileSystem.AddFile(FileSys.Path.Combine("/temp", $"{optionsName}.options"), yaml);

        var optionsSnapshot = _sp.GetRequiredService<IOptionsSnapshot<TestOptions>>();

        // Act
        var options = optionsSnapshot.Get(optionsName);

        // Assert
        options.Name
            .Should()
            .Be(expectedName);

        options.IntValue
            .Should()
            .Be(42);

        options.IsEnabled
            .Should()
            .BeTrue();
    }

    [Fact]
    public void GetOptionsFromFileSystem_ViaIOptionsSnapshotAfterWrite_ReturnsOptionsFromLastWrite()
    {
        // Arrange
        const string optionsName = "test_name";
        const string expectedName = "Test";

        var yaml =
            $"Name: Test1{Environment.NewLine}IntValue: 42{Environment.NewLine}IsEnabled: true{Environment.NewLine}";

        _fileSystem.AddFile(FileSys.Path.Combine("/temp", $"{optionsName}.options"), yaml);

        var optionsStorageProvider = _sp.GetRequiredService<IOptionsStorageProvider<TestOptions>>();

        optionsStorageProvider.Write(optionsName, new TestOptions { Name = expectedName });

        var optionsSnapshot = _sp.GetRequiredService<IOptionsSnapshot<TestOptions>>();

        // Act
        var options = optionsSnapshot.Get(optionsName);

        // Assert
        options.Name
            .Should()
            .Be(expectedName);

        options.IntValue
            .Should()
            .Be(0);

        options.IsEnabled
            .Should()
            .BeFalse();
    }

    [Fact]
    public void GetOptions_FileIsDeletedAfterPreviousWrite_PassedOptionsHasDefaultPropertyValues()
    {
        // Arrange
        const string optionsName = "test_name";
        const string expectedName = "Test";

        var yaml =
            $"Name: Test1{Environment.NewLine}IntValue: 42{Environment.NewLine}IsEnabled: true{Environment.NewLine}";

        _fileSystem.AddFile(FileSys.Path.Combine("/temp", $"{optionsName}.options"), yaml);

        var optionsStorageProvider = _sp.GetRequiredService<IOptionsStorageProvider<TestOptions>>();

        optionsStorageProvider.Write(optionsName, new TestOptions { Name = expectedName });

        _fileSystem.RemoveFile(FileSys.Path.Combine("/temp", $"{optionsName}.options"));

        var optionsSnapshot = _sp.GetRequiredService<IOptionsSnapshot<TestOptions>>();

        // Act
        var options = optionsSnapshot.Get(optionsName);

        // Assert
        options.Name
            .Should()
            .BeNull();

        options.IntValue
            .Should()
            .Be(0);

        options.IsEnabled
            .Should()
            .BeFalse();
    }
}
