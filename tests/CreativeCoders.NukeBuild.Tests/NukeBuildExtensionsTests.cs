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
        var expectedGitVersion = new GitVersion(
            BranchName: "main",
            Sha: "dummySha",
            ShortSha: "dummy",
            Major: 1,
            Minor: 0,
            Patch: 0,
            PreReleaseTag: string.Empty,
            PreReleaseTagWithDash: string.Empty,
            PreReleaseLabel: string.Empty,
            PreReleaseLabelWithDash: string.Empty,
            PreReleaseNumber: null,
            WeightedPreReleaseNumber: null,
            BuildMetaData: string.Empty,
            BuildMetaDataPadded: string.Empty,
            FullBuildMetaData: string.Empty,
            MajorMinorPatch: "1.0.0",
            SemVer: "1.0.0",
            LegacySemVer: "1.0.0",
            LegacySemVerPadded: "1.0.0",
            AssemblySemVer: "1.0.0.0",
            AssemblySemFileVer: "1.0.0.0",
            FullSemVer: "1.0.0",
            InformationalVersion: "1.0.0+dummySha",
            EscapedBranchName: "main",
            CommitDate: DateTimeOffset.Now.ToString(),
            CommitsSinceVersionSource: "0",
            CommitsSinceVersionSourcePadded: "0000",
            UncommittedChanges: 0,
            VersionSourceSha: "dummySourceSha",
            NuGetVersionV2: "1.0.0",
            NuGetVersion: "1.0.0",
            NuGetPreReleaseTagV2: string.Empty,
            NuGetPreReleaseTag: string.Empty
        );
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
