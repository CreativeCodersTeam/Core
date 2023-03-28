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
[GitHubActions("integration", GitHubActionsImage.WindowsLatest,
    OnPushBranches = new[]{"feature/**"},
    InvokedTargets = new []{"clean", "restore", "compile", "test", "codecoveragereport", "pack", "pushnuget"},
    PublishArtifacts = true)]
class Build : NukeBuild,
    ISolutionParameter,
    IGitRepositoryParameter,
    IConfigurationParameter,
    IGitVersionParameter,
    ISourceDirectoryParameter,
    IArtifactsSettings,
    ICleanTarget, ICompileTarget, IRestoreTarget, ITestTarget, ICodeCoverageReportTarget, IPackTarget, IPushNuGetTarget
{
    public static int Main() => Execute<Build>(x => ((ICodeCoverageReportTarget)x).CodeCoverageReport);

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

    string IPackSettings.PackageProjectUrl => "https://github.com/CreativeCodersTeam/Core";

    string IPackSettings.PackageLicenseExpression => PackageLicenseExpressions.ApacheLicense20;
    
    string IPackSettings.Copyright => $"{DateTime.Now.Year} CreativeCoders";
}
