using System;
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

[PublicAPI]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild, IBuildInfo,
    ISolutionParameter,
    IConfigurationParameter,
    IGitVersionParameter,
    ISourceDirectoryParameter,
    ICleanTarget, ICompileTarget, IRestoreTarget
{

    public Build()
    {
        //(this as ISourceDirectoryParameter).SourceDirectory
    }

    CleanTargetSettings ICleanTarget.ConfigureCleanSettings(CleanTargetSettings settings)
    {
        return (this as ICleanTarget).ConfigureDefaultCleanSettings(settings)
            .AddDirectoryForClean(((AbsolutePath) @"C:\temp\nextjs")!);
    }
    
    public static int Main() => Execute<Build>(x => x.RunBuild);

    // [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    // readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Parameter] string DevNuGetSource;

    [Parameter] string DevNuGetApiKey;

    [Parameter] string NuGetSource;

    [Parameter] string NuGetApiKey;

    //[Solution] readonly Solution Solution;

    [GitRepository] readonly GitRepository GitRepository;

    [GitVersion] readonly GitVersion GitVersion;

    //AbsolutePath SourceDirectory => RootDirectory / "source";

    AbsolutePath ArtifactsDirectory => RootDirectory / ".artifacts";

    AbsolutePath TestBaseDirectory => RootDirectory / ".tests";

    AbsolutePath TestResultsDirectory => TestBaseDirectory / "results";

    AbsolutePath TestProjectsBasePath => (this as ISourceDirectoryParameter).SourceDirectory / "UnitTests";

    AbsolutePath CoverageDirectory => TestBaseDirectory / "coverage";

    AbsolutePath TempNukeDirectory => RootDirectory / ".nuke" / "temp";

    const string PackageProjectUrl = "https://github.com/CreativeCodersTeam/Core";

    // Target Restore => _ => _
    //     .Before((this as ICompileTarget).Compile)
    //     .UseBuildAction<RestoreBuildAction>(this);

    Target Test => _ => _
        .After((this as ICompileTarget).Compile)
        .UseBuildAction<DotNetTestAction>(this,
            x => x
                .SetTestProjectsBaseDirectory(TestProjectsBasePath)
                .SetProjectsPattern("**/*.csproj")
                .SetResultsDirectory(TestResultsDirectory)
                .UseLogger("trx")
                .SetResultFileExt("trx")
                .EnableCoverage()
                .SetCoverageDirectory(CoverageDirectory));

    Target CoverageReport => _ => _
        .After(Test)
        .UseBuildAction<CoverageReportAction>(this,
            x => x
                .SetReports(TestBaseDirectory / "coverage" / "**" / "*.xml")
                .SetTargetDirectory(TestBaseDirectory / "coverage_report"));

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

    Target RunTest => _ => _
        .DependsOn(RunBuild)
        .DependsOn(Test)
        .DependsOn(CoverageReport);

    Target CreateNuGetPackages => _ => _
        .DependsOn(RunTest)
        .DependsOn(Pack);

    Target DeployToDevNuGet => _ => _
        .DependsOn(CreateNuGetPackages)
        .DependsOn(PushToDevNuGet);

    Target DeployToNuGet => _ => _
        .DependsOn(CreateNuGetPackages)
        .DependsOn(PushToNuGet);

    string IBuildInfo.Configuration => (this as IConfigurationParameter).Configuration;

    Solution IBuildInfo.Solution => (this as ISolutionParameter).Solution;

    GitRepository IBuildInfo.GitRepository => GitRepository;

    IVersionInfo IBuildInfo.VersionInfo => new GitVersionWrapper(GitVersion, "0.0.0", 1);

    AbsolutePath IBuildInfo.SourceDirectory => (this as ISourceDirectoryParameter).SourceDirectory;

    AbsolutePath IBuildInfo.ArtifactsDirectory => ArtifactsDirectory;
}
