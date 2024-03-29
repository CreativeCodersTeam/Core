﻿using CreativeCoders.NukeBuild.Components.Targets.Settings;
using JetBrains.Annotations;
using Nuke.Common;

namespace CreativeCoders.NukeBuild.Components.Targets;

[PublicAPI]
public interface ICreateGithubReleaseTarget : ICreateGithubReleaseSettings
{
    Target CreateGithubRelease => d => d
        .TryAfter<IPublishTarget>()
        .TryAfter<IBuildTarget>()
        .TryAfter<ICreateDistPackagesTarget>()
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
