using System;
using Nuke.Common.Tools.GitVersion;

#nullable enable

namespace CreativeCoders.NukeBuild;

public class GitVersionWrapper : IVersionInfo
{
    private readonly GitVersion? _gitVersion;

    private readonly string _defaultVersion;

    private readonly int _defaultBuildRevision;

    public GitVersionWrapper(GitVersion? gitVersion, string defaultVersion, int defaultBuildRevision)
    {
        _gitVersion = gitVersion;
        _defaultVersion = defaultVersion;
        _defaultBuildRevision = defaultBuildRevision;
    }

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
            () => _gitVersion?.AssemblySemVer,
            $"{_defaultVersion}.{_defaultBuildRevision}");

    public string GetAssemblySemFileVer()
        => SafeCallGitVersion(
            () => _gitVersion?.AssemblySemFileVer,
            $"{_defaultVersion}.{_defaultBuildRevision}");

    public string InformationalVersion
        => SafeCallGitVersion(
            () => _gitVersion?.InformationalVersion,
            $"{_defaultVersion}.{_defaultBuildRevision}-unknown");

    public string NuGetVersionV2
        => SafeCallGitVersion(
            () => _gitVersion?.NuGetVersionV2,
            $"{_defaultVersion}-unknown");
}
