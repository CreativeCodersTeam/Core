using CreativeCoders.Core;
using CreativeCoders.NukeBuild.Components.Parameters;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.GitVersion;

namespace CreativeCoders.NukeBuild.Components;

[PublicAPI]
public static class NukeBuildExtensions
{
    public static GitVersion? GetGitVersion(this INukeBuild build)
    {
        return build.As<IGitVersionParameter>()?.GitVersion;
    }

    public static AbsolutePath GetArtifactsDirectory(this INukeBuild build)
    {
        return build.As<IArtifactsSettings>()?.ArtifactsDirectory ??
               throw new InvalidOperationException("No artifacts directory specified");
    }

    public static Configuration GetConfiguration(this INukeBuild build)
    {
        return build.As<IConfigurationParameter>()?.Configuration ??
               (build.IsLocalBuild ? Configuration.Debug : Configuration.Release);
    }

    public static GitRepository GetGitRepository(this INukeBuild build)
    {
        return build.As<IGitRepositoryParameter>()?.GitRepository ??
               throw new InvalidOperationException("No GitRepository present");
    }

    public static Solution GetSolution(this INukeBuild build)
    {
        return build.As<ISolutionParameter>()?.Solution ??
               throw new InvalidOperationException("No Solution present");
    }

    public static void DisableAllTelemetry(this INukeBuild build)
    {
        build.DisableDotnetTelemetry();
        build.DisableNukeTelemetry();
    }

    public static void DisableDotnetTelemetry(this INukeBuild build)
    {
        Environment.SetEnvironmentVariable("DOTNET_CLI_TELEMETRY_OPTOUT", "1");
    }

    public static void DisableNukeTelemetry(this INukeBuild build)
    {
        Environment.SetEnvironmentVariable("NUKE_TELEMETRY_OPTOUT", "1");
    }
}
