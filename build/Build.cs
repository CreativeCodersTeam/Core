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
    OnPullRequestBranches = new[]{"main"},
    InvokedTargets = new []{"clean", "restore", "compile", "test", "codecoveragereport", "pack", "pushnuget"},
    ImportSecrets = new []{"NUGET_ORG_TOKEN"},
    EnableGitHubToken = true,
    PublishArtifacts = true,
    FetchDepth = 0
    )]
[GitHubActions("main", GitHubActionsImage.WindowsLatest,
    OnPushBranches = new[]{"main"},
    InvokedTargets = new []{"clean", "restore", "compile", "test", "codecoveragereport", "pack"},
    EnableGitHubToken = true,
    PublishArtifacts = true,
    FetchDepth = 0
)]
[GitHubActions("release", GitHubActionsImage.WindowsLatest,
    OnPushTags = new []{"refs/tags/v**"},
    InvokedTargets = new []{"clean", "restore", "compile", "test", "codecoveragereport", "pack", "pushnuget"},
    ImportSecrets = new []{"NUGET_ORG_TOKEN"},
    EnableGitHubToken = true,
    PublishArtifacts = true,
    FetchDepth = 0
)]
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

    [Parameter(Name = "GITHUB_TOKEN")] string DevNuGetApiKey;
    
    [Parameter(Name = "NUGET_ORG_TOKEN")] string NuGetOrgApiKey;

    GitHubActions GitHubActions = GitHubActions.Instance;
    
    string IPushNuGetSettings.NuGetFeedUrl =>
        GitHubActions.Ref.StartsWith("refs/tags/v", StringComparison.Ordinal)
            ? "nuget.org"
            : "https://nuget.pkg.github.com/CreativeCodersTeam/index.json";
    
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
