using System;
using System.Collections.Generic;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.NukeBuild;
using CreativeCoders.NukeBuild.BuildActions;
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
using Project = Nuke.Common.ProjectModel.Project;

[PublicAPI]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild,
    ISolutionParameter,
    IConfigurationParameter,
    IGitVersionParameter,
    ISourceDirectoryParameter,
    IArtifactsSettings,
    ICleanTarget, ICompileTarget, IRestoreTarget, ITestTarget, ICodeCoverageReportTarget, IPackTarget
{

    IList<AbsolutePath> ICleanSettings.DirectoriesToClean =>
        this.As<ICleanSettings>().DefaultDirectoriesToClean
            .AddRange(this.As<ITestSettings>().TestBaseDirectory);
    
    public static int Main() => Execute<Build>(x => ((ITestTarget)x).Test);

    public IEnumerable<Project> TestProjects => this.TryAs<ISolutionParameter>(out var solutionParameter)
        ? solutionParameter.Solution.GetProjects("*.UnitTests")
        : Array.Empty<Project>();

    // Target PushToDevNuGet => _ => _
    //     .Requires(() => DevNuGetSource)
    //     .Requires(() => DevNuGetApiKey)
    //     .UseBuildAction<PushBuildAction>(this,
    //         x => x
    //             .SetSource(DevNuGetSource)
    //             .SetApiKey(DevNuGetApiKey));

    // Target PushToNuGet => _ => _
    //     .Requires(() => NuGetApiKey)
    //     .UseBuildAction<PushBuildAction>(this,
    //         x => x
    //             .SetSource(NuGetSource)
    //             .SetApiKey(NuGetApiKey));
    //
    // Target RunBuild => _ => _
    //     .DependsOn((this as ICleanTarget).Clean)
    //     .DependsOn((this as ICompileTarget).Compile);

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
