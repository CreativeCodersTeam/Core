using System;
using System.Collections.Generic;
using System.Linq;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.NukeBuild;
using CreativeCoders.NukeBuild.BuildActions;
using CreativeCoders.NukeBuild.Components;
using CreativeCoders.NukeBuild.Components.Parameters;
using CreativeCoders.NukeBuild.Components.Targets;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.GitVersion;
using Octokit;
using Project = Nuke.Common.ProjectModel.Project;

[PublicAPI]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild, IBuildInfo,
    ISolutionParameter,
    IConfigurationParameter,
    IGitVersionParameter,
    ISourceDirectoryParameter,
    ICleanTarget, ICompileTarget, IRestoreTarget, ITestTarget
{

    IList<AbsolutePath> ICleanTargetSettings.DirectoriesToClean =>
        this.As<ICleanTargetSettings>().DefaultDirectoriesToClean
            .AddRange(this.As<ITestTargetSettings>().TestBaseDirectory);
    
    public static int Main() => Execute<Build>(x => x.RunBuild);

    [Parameter] string DevNuGetSource;

    [Parameter] string DevNuGetApiKey;

    [Parameter] string NuGetSource;

    [Parameter] string NuGetApiKey;

    //[Solution] readonly Solution Solution;

    [GitRepository] readonly GitRepository GitRepository;

    [GitVersion] readonly GitVersion GitVersion;

    //AbsolutePath SourceDirectory => RootDirectory / "source";

    AbsolutePath ArtifactsDirectory => RootDirectory / ".artifacts";

    //AbsolutePath TestBaseDirectory => RootDirectory / ".tests";

    //AbsolutePath TestResultsDirectory => TestBaseDirectory / "results";

    AbsolutePath TestProjectsBasePath => (this as ISourceDirectoryParameter).SourceDirectory / "UnitTests";

    //AbsolutePath CoverageDirectory => TestBaseDirectory / "coverage";

    AbsolutePath TempNukeDirectory => RootDirectory / ".nuke" / "temp";

    const string PackageProjectUrl = "https://github.com/CreativeCodersTeam/Core";

    // Target Test => _ => _
    //     .After((this as ICompileTarget).Compile)
    //     .UseBuildAction<DotNetTestAction>(this,
    //         x => x
    //             .SetTestProjectsBaseDirectory(TestProjectsBasePath)
    //             .SetProjectsPattern("**/*.csproj")
    //             .SetResultsDirectory(TestResultsDirectory)
    //             .UseLogger("trx")
    //             .SetResultFileExt("trx")
    //             .EnableCoverage()
    //             .SetCoverageDirectory(CoverageDirectory));

    public IEnumerable<Project> TestProjects => this.TryAs<ISolutionParameter>(out var solutionParameter)
        ? solutionParameter.Solution.GetProjects("*.UnitTests")
        : Array.Empty<Project>();

    // Target CoverageReport => _ => _
    //     .After<ITestTarget>()
    //     .UseBuildAction<CoverageReportAction>(this,
    //         x => x
    //             .SetReports(TestBaseDirectory / "coverage" / "**" / "*.xml")
    //             .SetTargetDirectory(TestBaseDirectory / "coverage_report"));

    Target Pack => _ => _
        .After((this as ICompileTarget).Compile)
        .UseBuildAction<PackBuildAction>(this,
            x => x
                .SetPackageLicenseExpression(PackageLicenseExpressions.ApacheLicense20)
                .SetPackageProjectUrl(PackageProjectUrl)
                .SetCopyright($"{DateTime.Now.Year} CreativeCoders")
                .SetEnableNoBuild(false));

    Target PushToDevNuGet => _ => _
        .Requires(() => DevNuGetSource)
        .Requires(() => DevNuGetApiKey)
        .UseBuildAction<PushBuildAction>(this,
            x => x
                .SetSource(DevNuGetSource)
                .SetApiKey(DevNuGetApiKey));

    Target PushToNuGet => _ => _
        .Requires(() => NuGetApiKey)
        .UseBuildAction<PushBuildAction>(this,
            x => x
                .SetSource(NuGetSource)
                .SetApiKey(NuGetApiKey));

    Target RunBuild => _ => _
        .DependsOn((this as ICleanTarget).Clean)
        .DependsOn((this as ICompileTarget).Compile);

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

    string IBuildInfo.Configuration => (this as IConfigurationParameter).Configuration;

    Solution IBuildInfo.Solution => (this as ISolutionParameter).Solution;

    GitRepository IBuildInfo.GitRepository => GitRepository;

    IVersionInfo IBuildInfo.VersionInfo => new GitVersionWrapper(GitVersion, "0.0.0", 1);

    AbsolutePath IBuildInfo.SourceDirectory => (this as ISourceDirectoryParameter).SourceDirectory;

    AbsolutePath IBuildInfo.ArtifactsDirectory => ArtifactsDirectory;
}
