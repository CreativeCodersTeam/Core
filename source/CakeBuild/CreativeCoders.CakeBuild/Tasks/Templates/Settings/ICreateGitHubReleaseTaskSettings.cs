namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

public interface ICreateGitHubReleaseTaskSettings : IBuildContextAccessor
{
    public string GitHubToken => Context.Environment.GetEnvironmentVariable("GITHUB_TOKEN");

    string ReleaseName { get; }

    string ReleaseBody => string.Empty;

    bool IsPreRelease => false;

    string ReleaseVersion { get; }

    IEnumerable<GitHubReleaseAsset> ReleaseAssets => [];
}
