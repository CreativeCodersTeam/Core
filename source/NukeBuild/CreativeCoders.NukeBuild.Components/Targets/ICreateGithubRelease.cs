using CreativeCoders.NukeBuild.Components.Targets.Settings;
using JetBrains.Annotations;
using Nuke.Common;

namespace CreativeCoders.NukeBuild.Components.Targets;

[PublicAPI]
public interface ICreateGithubRelease : ICreateGithubReleaseSettings
{
    Target CreateGithubRelease => _ => _
        .Executes(async () =>
        {
            await new GithubReleaseTasks(GithubToken)
                .CreateReleaseAsync(
                    ReleaseVersion,
                    ReleaseName,
                    ReleaseBody,
                    IsPreRelease,
                    ReleaseAssets)
                .ConfigureAwait(false);
        });
}
