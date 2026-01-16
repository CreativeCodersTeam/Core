namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

public interface ICreateGitHubReleaseTaskSettings
{
    string ReleaseName { get; }

    string ReleaseBody => string.Empty;

    bool IsPreRelease => false;

    string ReleaseVersion { get; }

    IEnumerable<GitHubReleaseAsset> ReleaseAssets => [];
}
