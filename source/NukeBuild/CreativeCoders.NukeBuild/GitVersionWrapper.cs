using System;
using Nuke.Common.Tools.GitVersion;

#nullable enable

namespace CreativeCoders.NukeBuild;

public class GitVersionWrapper(GitVersion? gitVersion, string defaultVersion, int defaultBuildRevision)
    : IVersionInfo
{
    private static string SafeCallGitVersion(Func<string?> callGitVersion, string defaultGitVersion)
    {
        try
        {
            return callGitVersion() ?? defaultGitVersion;
        }
        catch (Exception)
        {
            return defaultGitVersion;
        }
    }

    public string GetAssemblySemVer()
        => SafeCallGitVersion(
            () => gitVersion?.AssemblySemVer,
            $"{defaultVersion}.{defaultBuildRevision}");

    public string GetAssemblySemFileVer()
        => SafeCallGitVersion(
            () => gitVersion?.AssemblySemFileVer,
            $"{defaultVersion}.{defaultBuildRevision}");

    public string InformationalVersion
        => SafeCallGitVersion(
            () => gitVersion?.InformationalVersion,
            $"{defaultVersion}.{defaultBuildRevision}-unknown");

    public string NuGetVersionV2
        => SafeCallGitVersion(
            () => gitVersion?.NuGetVersionV2,
            $"{defaultVersion}-unknown");
}
