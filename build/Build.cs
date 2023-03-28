using System;
using System.Collections.Generic;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.NukeBuild.BuildActions;
using CreativeCoders.NukeBuild.Components.Parameters;
using CreativeCoders.NukeBuild.Components.Targets;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

[PublicAPI]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild,
    ISolutionParameter,
    IGitRepositoryParameter,
    IConfigurationParameter,
    IGitVersionParameter,
    ISourceDirectoryParameter,
    IArtifactsSettings,
    ICleanTarget, ICompileTarget, IRestoreTarget, ITestTarget, ICodeCoverageReportTarget, IPackTarget, IPushNuGetTarget
{
    public static int Main() => Execute<Build>(x => ((ITestTarget)x).Test);

    [Parameter(Name = "DevNuGetFeedUrl")] string DevNuGetFeedUrl;
    
    [Parameter(Name = "DevNuGetApiKey")] string DevNuGetApiKey;
    
    [Parameter(Name = "NuGetOrgFeedUrl")] string NuGetOrgFeedUrl;
    
    [Parameter(Name = "NuGetOrgApiKey")] string NuGetOrgApiKey;

    GitHubActions GitHubActions = GitHubActions.Instance;
    
    string IPushNuGetSettings.NuGetFeedUrl =>
        GitHubActions.Ref.StartsWith("refs/tags/v", StringComparison.Ordinal)
            ? NuGetOrgFeedUrl
            : DevNuGetFeedUrl;
    
    string IPushNuGetSettings.NuGetApiKey =>
        GitHubActions.Ref.StartsWith("refs/tags/v", StringComparison.Ordinal)
            ? NuGetOrgApiKey
            : DevNuGetApiKey;
    
    IList<AbsolutePath> ICleanSettings.DirectoriesToClean =>
        this.As<ICleanSettings>().DefaultDirectoriesToClean
            .AddRange(this.As<ITestSettings>().TestBaseDirectory);
    
    public IEnumerable<Project> TestProjects => this.TryAs<ISolutionParameter>(out var solutionParameter)
        ? solutionParameter.Solution.GetProjects("*.UnitTests")
        : Array.Empty<Project>();

    // Target PushToNuGet => _ => _
    //     .Requires(() => NuGetApiKey)
    //     .UseBuildAction<PushBuildAction>(this,
    //         x => x
    //             .SetSource(NuGetSource)
    //             .SetApiKey(NuGetApiKey));
    //
    // Target RunTest => _ => _
    //     .DependsOn(RunBuild)
    //     .DependsOn<ITestTarget>()
    //     .DependsOn(CoverageReport);

    // Target CreateNuGetPackages => _ => _
    //     .DependsOn(RunTest)
    //     .DependsOn(Pack);

    // Target DeployToDevNuGet => _ => _
    //     .DependsOn(CreateNuGetPackages)
    //     .DependsOn(PushToDevNuGet);

    // Target DeployToNuGet => _ => _
    //     .DependsOn(CreateNuGetPackages)
    //     .DependsOn(PushToNuGet);

    string IPackSettings.PackageProjectUrl => "https://github.com/CreativeCodersTeam/Core";

    string IPackSettings.PackageLicenseExpression => PackageLicenseExpressions.ApacheLicense20;
    
    string IPackSettings.Copyright => $"{DateTime.Now.Year} CreativeCoders";
}
