using Cake.Common.Build;
using Cake.Common.Diagnostics;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using CreativeCoders.Core;
using Octokit;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

public class
    CreateGitHubReleaseTask<TBuildContext>(IGitHubClient gitHubClient)
    : FrostingTaskBase<TBuildContext, ICreateGitHubReleaseTaskSettings>
    where TBuildContext : CakeBuildContext
{
    private readonly IGitHubClient _gitHubClient = Ensure.NotNull(gitHubClient);

    protected override async Task RunAsyncCore(TBuildContext context,
        ICreateGitHubReleaseTaskSettings taskSettings)
    {
        await CreateReleaseAsync(context, taskSettings).ConfigureAwait(false);
    }

    private async Task CreateReleaseAsync(TBuildContext context,
        ICreateGitHubReleaseTaskSettings taskSettings)
    {
        var release = await CreateReleaseDraftAsync(context,
                taskSettings.ReleaseVersion,
                taskSettings.ReleaseName,
                taskSettings.ReleaseBody,
                taskSettings.IsPreRelease)
            .ConfigureAwait(false);

        await UploadReleaseAssets(release, taskSettings.ReleaseAssets).ConfigureAwait(false);

        await _gitHubClient.Repository.Release
            .Edit(context.GitHubActions().Environment.Workflow.RepositoryOwner, GetRepositoryName(context),
                release.Id,
                new ReleaseUpdate { Draft = false }).ConfigureAwait(false);
    }

    private async Task UploadReleaseAssets(Release release,
        IEnumerable<GitHubReleaseAsset> githubReleaseAssets)
    {
        foreach (var releaseAsset in githubReleaseAssets)
        {
            var dataStream = releaseAsset.GetAssetStream();

            await using var dataStreamDisposable = dataStream.ConfigureAwait(false);

            var releaseAssetUpload = new ReleaseAssetUpload
            {
                ContentType = releaseAsset.ContentType,
                FileName = releaseAsset.AssetFileName,
                RawData = dataStream
            };

            _ = await _gitHubClient.Repository.Release.UploadAsset(release, releaseAssetUpload)
                .ConfigureAwait(false);
        }
    }

    private async Task<Release> CreateReleaseDraftAsync(TBuildContext context, string releaseVersion,
        string name, string body,
        bool isPreRelease)
    {
        var repositoryOwner = context.GitHubActions().Environment.Workflow.RepositoryOwner;
        var repositoryName = GetRepositoryName(context);

        context.Debug("Create github release");
        context.Debug("Repo Owner: {RepositoryOwner}", repositoryOwner);
        context.Debug("Repo name: {RepositoryName}", repositoryName);
        context.Debug("Version: {ReleaseVersion}", releaseVersion);
        context.Debug("Name: {Name}", name);
        context.Debug("Body: {Body}", body);

        return await _gitHubClient.Repository.Release
            .Create(repositoryOwner, repositoryName,
                new NewRelease(releaseVersion)
                {
                    Name = name,
                    Body = body,
                    Draft = true,
                    Prerelease = isPreRelease
                })
            .ConfigureAwait(false);
    }

    private static string GetRepositoryName(TBuildContext context)
    {
        var repositoryName = context.GitHubActions().Environment.Workflow.Repository;
        var index = repositoryName.LastIndexOf('/');

        return index > -1
            ? repositoryName[(index + 1)..]
            : throw new InvalidOperationException(
                $"No repository name found in '{repositoryName}'");
    }
}
