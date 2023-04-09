using Nuke.Common;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

public interface ICreateGithubReleaseSettings : INukeBuild
{
    [Parameter(Name = "GITHUB_TOKEN")]
    string GithubToken => TryGetValue(() => GithubToken) ?? string.Empty;

    string ReleaseName { get; }

    string ReleaseBody { get; }

    bool IsPreRelease => false;

    string ReleaseVersion { get; }

    IEnumerable<GithubReleaseAsset> ReleaseAssets { get; }
}
