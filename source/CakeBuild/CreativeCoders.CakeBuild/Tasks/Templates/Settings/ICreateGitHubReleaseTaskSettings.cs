namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

public interface ICreateGitHubReleaseTaskSettings : IBuildContextAccessor
{
    /// <summary>
    /// Gets the GitHub access token used for creating the release.
    /// </summary>
    /// <remarks>
    /// This value is read from the <c>GITHUB_TOKEN</c> environment variable and is highly sensitive.
    /// Implementers and consumers must ensure that this token is kept secure and is never written
    /// to console output, logs, or other persistent storage in plain text.
    /// </remarks>
    public string GitHubToken => Context.Environment.GetEnvironmentVariable("GITHUB_TOKEN");

    string ReleaseName { get; }

    string ReleaseBody => string.Empty;

    bool IsPreRelease => false;

    string ReleaseVersion { get; }

    IEnumerable<GitHubReleaseAsset> ReleaseAssets => [];
}
