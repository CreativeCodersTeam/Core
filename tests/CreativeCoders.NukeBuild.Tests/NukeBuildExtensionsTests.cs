using System.Diagnostics.CodeAnalysis;
using CreativeCoders.NukeBuild.Components;
using FluentAssertions;
using JetBrains.Annotations;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitVersion;

namespace CreativeCoders.NukeBuild.Tests;

[TestSubject(typeof(NukeBuildExtensions))]
public class NukeBuildExtensionsTests
{
    [Fact]
    public void GetGitVersion_Always_ReturnsVersionFromGitVersionParameter()
    {
        // Arrange
        var expectedGitVersion = new GitVersion();
        var fakeBuild = new MockBuildWithGitVersionParameter(expectedGitVersion);

        // Act
        var actualGitVersion = fakeBuild.GetGitVersion();

        // Assert
        actualGitVersion
            .Should()
            .BeSameAs(expectedGitVersion);
    }

    [Fact]
    public void GetArtifactsDirectory_Always_ReturnsPathFromArtifactsSettings()
    {
        // Arrange
        var expectedPath = AbsolutePath.Create("/path/to/artifacts");
        var fakeBuild = new MockBuildWithArtifactsSettings(expectedPath);

        // Act
        var actualPath = fakeBuild.GetArtifactsDirectory();

        // Assert
        actualPath
            .Should()
            .BeSameAs(expectedPath);
    }

    [Fact]
    [SuppressMessage("ReSharper", "NullableWarningSuppressionIsUsed")]
    public void GetArtifactsDirectory_WhenNoArtifactsSettings_ThrowsException()
    {
        // Arrange
        var fakeBuild = new MockBuildWithArtifactsSettings(null!);

        // Act
        var act = () => fakeBuild.GetArtifactsDirectory();

        // Assert
        act
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("No artifacts directory specified");
    }
}
