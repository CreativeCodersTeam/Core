﻿using CreativeCoders.Core;
using CreativeCoders.Core.SysEnvironment;
using CreativeCoders.NukeBuild.Components.Parameters;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitVersion;

namespace CreativeCoders.NukeBuild.Components;

public static class NukeBuildExtensions
{
    public static bool IsRunnerOs(this INukeBuild build, string runnerOs)
    {
        return Env.GetEnvironmentVariable("RUNNER_OS")?
            .Equals(runnerOs, StringComparison.OrdinalIgnoreCase) == true;
    }

    public static GitVersion? GetGitVersion(this INukeBuild build)
    {
        return build.As<IGitVersionParameter>()?.GitVersion;
    }

    public static AbsolutePath GetArtifactsDirectory(this INukeBuild build)
    {
        return build.As<IArtifactsSettings>()?.ArtifactsDirectory ??
               throw new InvalidOperationException("No artifacts directory specified");
    }
}
