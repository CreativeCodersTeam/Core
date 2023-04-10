﻿using CreativeCoders.NukeBuild.Components.Targets.Settings;
using JetBrains.Annotations;
using Nuke.Common;

namespace CreativeCoders.NukeBuild.Components.Targets;

[PublicAPI]
public interface ICreateGithubReleaseTarget : ICreateGithubReleaseSettings
{
    Target CreateGithubRelease => _ => _
        .TryAfter<IPublishTarget>()
        .TryAfter<ICompileTarget>()
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
