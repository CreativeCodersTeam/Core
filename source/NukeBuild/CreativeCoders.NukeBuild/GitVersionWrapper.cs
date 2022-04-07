using System;
using Nuke.Common.Tools.GitVersion;

namespace CreativeCoders.NukeBuild;

public class GitVersionWrapper : IVersionInfo
{
    private readonly GitVersion _gitVersion;

    private readonly string _defaultVersion;

    private readonly int _defaultBuildRevision;

    public GitVersionWrapper(GitVersion gitVersion, string defaultVersion, int defaultBuildRevision)
    {
        _gitVersion = gitVersion;
        _defaultVersion = defaultVersion;
        _defaultBuildRevision = defaultBuildRevision;
    }

    private string SafeCallGitVersion(Func<string> callGitVersion)
    {
        try
        {
            return callGitVersion();
        }
        catch (Exception)
        {
            return _defaultVersion;
        }
    }

    public string GetAssemblySemVer()
    {
        return SafeCallGitVersion(() => _gitVersion?.AssemblySemVer) ??
               $"{_defaultVersion}.{_defaultBuildRevision}";
    }

    public string GetAssemblySemFileVer()
    {
        return SafeCallGitVersion(() => _gitVersion?.AssemblySemFileVer) ??
               $"{_defaultVersion}.{_defaultBuildRevision}";
    }

    public string InformationalVersion => _gitVersion?.InformationalVersion ??
                                          $"{_defaultVersion}.{_defaultBuildRevision}-unknown";

    public string NuGetVersionV2 => _gitVersion?.NuGetVersionV2 ?? $"{_defaultVersion}-unknown";
}
