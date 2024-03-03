namespace CreativeCoders.NukeBuild.Components.Targets;

public static class NukeTargets
{
    public const string Restore = nameof(IRestoreTarget.Restore);

    public const string Clean = nameof(ICleanTarget.Clean);

    public const string Build = nameof(IBuildTarget.Build);

    public const string Rebuild = nameof(IRebuildTarget.Rebuild);

    public const string Test = nameof(ITestTarget.Test);

    public const string CodeCoverage = nameof(ICodeCoverageTarget.CodeCoverage);

    public const string Pack = nameof(IPackTarget.Pack);

    public const string PushNuGet = nameof(IPushNuGetTarget.PushNuGet);

    public const string DeployNuGet = nameof(IDeployNuGetTarget.DeployNuGet);

    public const string Publish = nameof(IPublishTarget.Publish);

    public const string CreateDistPackages = nameof(ICreateDistPackagesTarget.CreateDistPackages);

    public const string CreateGitHubRelease = nameof(ICreateGithubReleaseTarget.CreateGithubRelease);
}
