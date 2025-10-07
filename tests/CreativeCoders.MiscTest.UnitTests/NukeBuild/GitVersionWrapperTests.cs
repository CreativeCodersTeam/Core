using CreativeCoders.NukeBuild;
using FakeItEasy;
using AwesomeAssertions;
using Nuke.Common.Tools.GitVersion;
using Xunit;

namespace CreativeCoders.MiscTest.UnitTests.NukeBuild;

public class GitVersionWrapperTests
{
    [Theory]
    [InlineData("1", 1, 1, 1, 0, "")]
    [InlineData("1.0", 1, 1, 0, 1, "")]
    [InlineData("1.0.1", 1, 1, 1, 1, "1")]
    public void GetAssemblySemVer_GitVersionThrowsException_ReturnsDefaultVersion(
        string defaultVersion, int defaultBuildRevision, int expectedMajor,
        int expectedMinor, int expectedPatch, string expectedBuildMetaData)
    {
        var gitVersion = CreateGitVersion(expectedMajor, expectedMinor, expectedPatch, expectedBuildMetaData);

        var versionWrapper = new GitVersionWrapper(gitVersion, defaultVersion, defaultBuildRevision);

        // Act
        var actualVersion = versionWrapper.GetAssemblySemVer();

        // Assert
        actualVersion
            .Should()
            .Be($"{expectedMajor}.{expectedMinor}.{expectedPatch}-{expectedBuildMetaData}");
    }

    [Theory]
    [InlineData("1", 1, 1, 1, 0, "")]
    [InlineData("1.0", 1, 1, 0, 1, "")]
    [InlineData("1.0.1", 1, 1, 1, 1, "1")]
    public void GetAssemblySemVer_GitVersionReturnsNull_ReturnsDefaultVersion(
        string defaultVersion, int defaultBuildRevision, int expectedMajor,
        int expectedMinor, int expectedPatch, string expectedBuildMetaData)
    {
        var gitVersion = CreateGitVersion(expectedMajor, expectedMinor, expectedPatch, expectedBuildMetaData);

        var versionWrapper = new GitVersionWrapper(gitVersion, defaultVersion, defaultBuildRevision);

        // Act
        var actualVersion = versionWrapper.GetAssemblySemVer();

        // Assert
        actualVersion
            .Should()
            .Be($"{expectedMajor}.{expectedMinor}.{expectedPatch}-{expectedBuildMetaData}");
    }

    [Theory]
    [InlineData(1, 1, 0, "")]
    [InlineData(1, 0, 1, "")]
    [InlineData(1, 1, 1, "1")]
    public void GetAssemblySemVer_GitVersionReturnsVersion_ReturnsVersion(int expectedMajor,
        int expectedMinor, int expectedPatch, string expectedBuildMetaData)
    {
        var gitVersion = CreateGitVersion(expectedMajor, expectedMinor, expectedPatch, expectedBuildMetaData);

        var versionWrapper = new GitVersionWrapper(gitVersion, "2.0", 2);

        // Act
        var actualVersion = versionWrapper.GetAssemblySemVer();

        // Assert
        actualVersion
            .Should()
            .Be($"{expectedMajor}.{expectedMinor}.{expectedPatch}-{expectedBuildMetaData}");
    }

    [Theory]
    [InlineData("1", 1)]
    [InlineData("1.0", 1)]
    [InlineData("1.0.1", 1)]
    public void GetAssemblySemFileVer_GitVersionReturnsNull_ReturnsDefaultVersion(
        string defaultVersion, int defaultBuildRevision)
    {
        var gitVersion = CreateGitVersionWithEmptyProps();

        var versionWrapper = new GitVersionWrapper(gitVersion, defaultVersion, defaultBuildRevision);

        // Act
        var actualVersion = versionWrapper.GetAssemblySemFileVer();

        // Assert
        actualVersion
            .Should()
            .Be(defaultVersion + $".{defaultBuildRevision}");
    }

    [Theory]
    [InlineData(1, 0, 0, "")]
    [InlineData(1, 2, 3, "b12")]
    public void GetAssemblySemFileVer_GitVersionReturnsVersion_ReturnsVersion(int expectedMajor,
        int expectedMinor, int expectedPatch, string expectedBuildMetaData)
    {
        var gitVersion = CreateGitVersion(expectedMajor, expectedMinor, expectedPatch, expectedBuildMetaData);

        var versionWrapper = new GitVersionWrapper(gitVersion, "2.0", 2);

        // Act
        var actualVersion = versionWrapper.GetAssemblySemFileVer();

        // Assert
        actualVersion
            .Should()
            .Be($"{expectedMajor}.{expectedMinor}.{expectedPatch}-{expectedBuildMetaData}");
    }

    [Theory]
    [InlineData("1", 1, "1.1-unknown")]
    [InlineData("1.0", 1, "1.0.1-unknown")]
    [InlineData("1.0.1", 1, "1.0.1.1-unknown")]
    public void InformationalVersion_GitVersionReturnsNull_ReturnsDefaultVersion(
        string defaultVersion, int defaultBuildRevision, string expectedVersion)
    {
        var gitVersion = CreateGitVersionWithEmptyProps();

        var versionWrapper = new GitVersionWrapper(gitVersion, defaultVersion, defaultBuildRevision);

        // Act
        var actualVersion = versionWrapper.InformationalVersion;

        // Assert
        actualVersion
            .Should()
            .Be(expectedVersion);
    }

    [Theory]
    [InlineData(1, 0, 0, "")]
    [InlineData(1, 2, 3, "b12")]
    public void InformationalVersion_GitVersionReturnsVersion_ReturnsVersion(int expectedMajor,
        int expectedMinor, int expectedPatch, string expectedBuildMetaData)
    {
        var gitVersion = CreateGitVersion(expectedMajor, expectedMinor, expectedPatch, expectedBuildMetaData);

        var versionWrapper = new GitVersionWrapper(gitVersion, "2.0", 2);

        // Act
        var actualVersion = versionWrapper.InformationalVersion;

        // Assert
        actualVersion
            .Should()
            .Be($"{expectedMajor}.{expectedMinor}.{expectedPatch}-{expectedBuildMetaData}");
    }

    [Theory]
    [InlineData("1", 1, "1-unknown")]
    [InlineData("1.0", 1, "1.0-unknown")]
    [InlineData("1.0.1", 1, "1.0.1-unknown")]
    public void NuGetVersionV2_GitVersionReturnsNull_ReturnsDefaultVersion(
        string defaultVersion, int defaultBuildRevision, string expectedVersion)
    {
        var gitVersion = CreateGitVersionWithEmptyProps();

        var versionWrapper = new GitVersionWrapper(gitVersion, defaultVersion, defaultBuildRevision);

        // Act
        var actualVersion = versionWrapper.NuGetVersionV2;

        // Assert
        actualVersion
            .Should()
            .Be(expectedVersion);
    }

    [Theory]
    [InlineData(1, 0, 0, "")]
    [InlineData(1, 2, 3, "b12")]
    public void NuGetVersionV2_GitVersionReturnsVersion_ReturnsVersion(int expectedMajor,
        int expectedMinor, int expectedPatch, string expectedBuildMetaData)
    {
        var gitVersion = CreateGitVersion(expectedMajor, expectedMinor, expectedPatch, expectedBuildMetaData);

        var versionWrapper = new GitVersionWrapper(gitVersion, "2.0", 2);

        // Act
        var actualVersion = versionWrapper.NuGetVersionV2;

        // Assert
        actualVersion
            .Should()
            .Be($"{expectedMajor}.{expectedMinor}.{expectedPatch}");
    }

    private GitVersion CreateGitVersionWithEmptyProps()
    {
        return new GitVersion(
            BranchName: "main",
            Sha: "dummySha",
            ShortSha: "dummy",
            Major: 0,
            Minor: 0,
            Patch: 0,
            PreReleaseTag: string.Empty,
            PreReleaseTagWithDash: string.Empty,
            PreReleaseLabel: string.Empty,
            PreReleaseLabelWithDash: string.Empty,
            PreReleaseNumber: null,
            WeightedPreReleaseNumber: null,
            BuildMetaData: null,
            BuildMetaDataPadded: string.Empty,
            FullBuildMetaData: string.Empty,
            MajorMinorPatch: null,
            SemVer: null,
            LegacySemVer: null,
            LegacySemVerPadded: null,
            AssemblySemVer: null,
            AssemblySemFileVer: null,
            FullSemVer: null,
            InformationalVersion: null,
            EscapedBranchName: "main",
            CommitDate: DateTimeOffset.Now.ToString(),
            CommitsSinceVersionSource: "0",
            CommitsSinceVersionSourcePadded: "0000",
            UncommittedChanges: 0,
            VersionSourceSha: "dummySourceSha",
            NuGetVersionV2: null,
            NuGetVersion: null,
            NuGetPreReleaseTagV2: string.Empty,
            NuGetPreReleaseTag: string.Empty
        );
    }

    private GitVersion CreateGitVersion(int major, int minor, int patch, string buildMetaData)
    {
        var shortVersion = $"{major}.{minor}.{patch}";
        var fullVersion = $"{major}.{minor}.{patch}-{buildMetaData}";

        return new GitVersion(
            BranchName: "main",
            Sha: "dummySha",
            ShortSha: "dummy",
            Major: major,
            Minor: minor,
            Patch: patch,
            PreReleaseTag: string.Empty,
            PreReleaseTagWithDash: string.Empty,
            PreReleaseLabel: string.Empty,
            PreReleaseLabelWithDash: string.Empty,
            PreReleaseNumber: null,
            WeightedPreReleaseNumber: null,
            BuildMetaData: buildMetaData,
            BuildMetaDataPadded: string.Empty,
            FullBuildMetaData: string.Empty,
            MajorMinorPatch: shortVersion,
            SemVer: shortVersion,
            LegacySemVer: shortVersion,
            LegacySemVerPadded: shortVersion,
            AssemblySemVer: fullVersion,
            AssemblySemFileVer: fullVersion,
            FullSemVer: shortVersion,
            InformationalVersion: fullVersion,
            EscapedBranchName: "main",
            CommitDate: DateTimeOffset.Now.ToString(),
            CommitsSinceVersionSource: "0",
            CommitsSinceVersionSourcePadded: "0000",
            UncommittedChanges: 0,
            VersionSourceSha: "dummySourceSha",
            NuGetVersionV2: shortVersion,
            NuGetVersion: shortVersion,
            NuGetPreReleaseTagV2: string.Empty,
            NuGetPreReleaseTag: string.Empty
        );
    }
}
