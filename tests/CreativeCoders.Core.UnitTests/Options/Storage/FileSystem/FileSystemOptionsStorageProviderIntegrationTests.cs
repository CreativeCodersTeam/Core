using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Abstractions;
using CreativeCoders.Core.IO;
using CreativeCoders.Options.Core;
using CreativeCoders.Options.Storage.FileSystem;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace CreativeCoders.Core.UnitTests.Options.Storage.FileSystem;

[Collection("FileSys")]
[SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
public class FileSystemOptionsStorageProviderIntegrationTests
{
    private readonly IOptionsStorageDataSerializer<TestOptions> _dataSerializer;

    private readonly IFile _file;

    private readonly IPath _path;

    private readonly ServiceCollection _services;

    public FileSystemOptionsStorageProviderIntegrationTests()
    {
        _services = [];

        var fileSystem = A.Fake<IFileSystemEx>();
        FileSys.InstallFileSystemSupport(fileSystem);

        _file = A.Fake<IFile>();
        A.CallTo(() => fileSystem.File).Returns(_file);

        _path = A.Fake<IPath>();
        A.CallTo(() => fileSystem.Path).Returns(_path);

        A.CallTo(() => _path.Combine(A<string>._, A<string>._))
            .ReturnsLazily(call => Path.Combine(call.GetArgument<string>(0) ?? string.Empty,
                call.GetArgument<string>(1) ?? string.Empty));

        _dataSerializer = A.Fake<IOptionsStorageDataSerializer<TestOptions>>();

        _services.AddSingleton(_dataSerializer);

        _services.AddFileSystemOptionsStorage<TestOptions>("TestDirectory");

        _services.AddNamedOptions<TestOptions>();
    }

    [Fact]
    public void GetOptionsMonitor_OptionsDataWrittenBetweenTwoCalls_ReturnsChangedOptions()
    {
        // Arrange
        const string serializedOptions = "SerializedOptions";
        const string optionsName = "test_name";
        const string expectedName1 = "Test";
        const string expectedName2 = "Test2";

        A.CallTo(() => _file.ReadAllText(Path.Combine("TestDirectory", "test_name.options")))
            .Returns(serializedOptions);

        A.CallTo(() => _dataSerializer.Deserialize(serializedOptions, A<TestOptions>.Ignored))
            .Invokes(call => call.GetArgument<TestOptions>(1)!.Name = expectedName1)
            .Once()
            .Then
            .Invokes(call => call.GetArgument<TestOptions>(1)!.Name = expectedName2);

        var serviceProvider = _services.BuildServiceProvider();

        var optionsSnapshots1 = serviceProvider.GetRequiredService<IOptionsMonitor<TestOptions>>();

        // Act
        var options1 = optionsSnapshots1.Get(optionsName);

        serviceProvider.GetRequiredService<IOptionsStorageProvider<TestOptions>>()
            .Write(optionsName, options1);

        var optionsSnapshots2 = serviceProvider.CreateScope().ServiceProvider
            .GetRequiredService<IOptionsMonitor<TestOptions>>();
        var options2 = optionsSnapshots2.Get(optionsName);

        // Assert
        options1.Name
            .Should()
            .Be(expectedName1);

        options2.Name
            .Should()
            .Be(expectedName2);
    }
}
