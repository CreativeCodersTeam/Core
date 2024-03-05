using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Core.SysEnvironment;
using CreativeCoders.NukeBuild.Components;
using FakeItEasy;
using FluentAssertions;
using JetBrains.Annotations;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitVersion;

namespace CreativeCoders.NukeBuild.Tests;

[TestSubject(typeof(NukeBuildExtensions))]
[Collection("Env")]
public class NukeBuildExtensionsTests
{
    [Theory]
    [InlineData("Windows", "Windows", true)]
    [InlineData("windows", "Windows", true)]
    [InlineData("Linux", "Windows", false)]
    [InlineData("macOS", "macos", true)]
    public void IsRunnerOs_Always_CallsGetEnvironmentVariableAndReturnsExpectedResult(
        string environmentVariableValue, string runnerOs, bool expectedResult)
    {
        // Arrange
        var fakeBuild = new MockNukeBuild();

        var env = A.Fake<IEnvironment>();
        Env.SetEnvironmentImpl(env);

        A.CallTo(() => env.GetEnvironmentVariable("RUNNER_OS"))
            .Returns(environmentVariableValue);

        // Act
        var actual = fakeBuild.IsGitHubActionsRunnerOs(runnerOs);

        // Assert
        A.CallTo(() => env.GetEnvironmentVariable("RUNNER_OS"))
            .MustHaveHappenedOnceExactly();

        actual
            .Should()
            .Be(expectedResult);
    }

    // Test for GetGitVersion method
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

    // Test for GetArtifactsDirectory method
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
