﻿using CreativeCoders.Core.IO;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using Nuke.Common.CI.GitHubActions;
using Octokit;
using Serilog;

namespace CreativeCoders.NukeBuild.Components;

public class GithubReleaseTasks
{
    private readonly GitHubClient _githubClient;

    public GithubReleaseTasks(string githubToken)
    {
        _githubClient = new GitHubClient(new ProductHeaderValue("CreativeCoders.Nuke"))
        {
            Credentials = new Credentials(githubToken)
        };
    }

    public async Task CreateReleaseAsync(string releaseVersion, string name, string body, bool isPreRelease, IEnumerable<GithubReleaseAsset> releaseAssets)
    {
        var release = await CreateReleaseDraftAsync(releaseVersion, name, body, isPreRelease).ConfigureAwait(false);

        await UploadReleaseAssets(release, releaseAssets).ConfigureAwait(false);

        await _githubClient.Repository.Release
            .Edit(GitHubActions.Instance.RepositoryOwner, GitHubActions.Instance.Repository, release.Id, new ReleaseUpdate { Draft = false });
    }

    private async Task UploadReleaseAssets(Release release,
        IEnumerable<GithubReleaseAsset> githubReleaseAssets)
    {
        foreach (var releaseAssetUpload in githubReleaseAssets.Select(releaseAsset => new ReleaseAssetUpload
                 {
                     ContentType = MimeMapping.MimeUtility.GetMimeMapping(releaseAsset.FileName),
                     FileName = FileSys.Path.GetFileName(releaseAsset.FileName),
                     RawData = FileSys.File.OpenRead(releaseAsset.FileName)
                 }))
        {
            var _ = await _githubClient.Repository.Release.UploadAsset(release, releaseAssetUpload);
        }
    }

    private async Task<Release> CreateReleaseDraftAsync(string releaseVersion, string name, string body, bool isPreRelease)
    {
        var repositoryOwner = GitHubActions.Instance.RepositoryOwner;
        var repositoryName = GetRepositoryName();

        Log.Debug($"Create github release");
        Log.Debug($"Repo Owner: {repositoryOwner}");
        Log.Debug($"Repo name: {repositoryName}");
        Log.Debug($"Version: {releaseVersion}");
        Log.Debug($"Name: {name}");
        Log.Debug($"Body: {body}");

        return await _githubClient.Repository.Release
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

    private static string GetRepositoryName()
    {
        var index = GitHubActions.Instance.Repository.LastIndexOf('/');

        return index > -1
            ? GitHubActions.Instance.Repository.Substring(index)
            : throw new ArgumentException("No repository name",
                nameof(GitHubActions.Instance.Repository));
    }
}
