using Cake.Common.Tools.GitVersion;

namespace CreativeCoders.CakeBuild;

public static class StaticGitVersion
{
    public static GitVersion Create(string major, string minor, string patch, string build,
        string preReleaseTag)
    {
        var version = $"{major}.{minor}.{patch}.{build}";
        var versionWithPreRelease = string.IsNullOrWhiteSpace(preReleaseTag)
            ? version
            : $"{version}-{preReleaseTag}";

        return new GitVersion
        {
            Major = int.Parse(major),
            Minor = int.Parse(minor),
            Patch = int.Parse(patch),
            PreReleaseTag = preReleaseTag,
            BuildMetaData = build,
            AssemblySemVer = versionWithPreRelease,
            FullSemVer = versionWithPreRelease,
            AssemblySemFileVer = version,
            BranchName = "unknown",
            InformationalVersion = versionWithPreRelease,
            MajorMinorPatch = $"{major}.{minor}.{patch}",
            LegacySemVer = versionWithPreRelease,
            NuGetVersion = versionWithPreRelease,
            NuGetVersionV2 = versionWithPreRelease,
            PreReleaseLabel = preReleaseTag,
            SemVer = versionWithPreRelease,
            NuGetPreReleaseTag = preReleaseTag,
            NuGetPreReleaseTagV2 = preReleaseTag,
            CommitDate = DateTimeOffset.Now.ToString(),
            PreReleaseTagWithDash = $"-{preReleaseTag}",
            PreReleaseLabelWithDash = $"-{preReleaseTag}"
        };
    }
}
