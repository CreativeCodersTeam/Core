using Cake.Common.Build;
using Cake.Core;
using Cake.Core.IO;
using CreativeCoders.CakeBuild;
using CreativeCoders.CakeBuild.Tasks.Defaults;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using JetBrains.Annotations;

namespace CakeBuildSample;

[UsedImplicitly]
public class SampleBuildContext(ICakeContext context)
    : CakeBuildContext(context), IDefaultTaskSettings, ICreateDistPackagesTaskSettings
{
    public IList<DirectoryPath> DirectoriesToClean => this.CastAs<ICleanTaskSettings>()
        .GetDefaultDirectoriesToClean().AddRange(RootDir.Combine(".tests"));


    public string Copyright => $"{DateTime.Now.Year} CreativeCoders";

    public string PackageProjectUrl => "https://github.com/CreativeCodersTeam/Core";

    public string PackageLicenseExpression => PackageLicenseExpressions.ApacheLicense20;

    public string NuGetFeedUrl => this.GitHubActions().Environment.Workflow.Workflow == "release"
        ? "nuget.org"
        : "https://nuget.pkg.github.com/CreativeCodersTeam/index.json";

    public bool SkipPush => this.BuildSystem().IsPullRequest ||
                            this.BuildSystem().IsLocalBuild ||
                            this.GitHubActions().Environment.Runner.OS != "Linux";

    public DirectoryPath PublishOutputDir => ArtifactsDir.Combine("publish");

    private const string CliHostSampleAppProjectName = "CliHostSampleApp";

    public IEnumerable<PublishingItem> PublishingItems =>
    [
        new PublishingItem(
            RootDir.Combine("samples").Combine("CliHostSampleApp")
                .CombineWithFilePath("CliHostSampleApp.csproj"),
            PublishOutputDir.Combine(CliHostSampleAppProjectName))
    ];

    public IEnumerable<DistPackage> DistPackages =>
    [
        new DistPackage("clihostsample", PublishOutputDir.Combine(CliHostSampleAppProjectName))
        {
            Format = DistPackageFormat.TarGz
        },
        new DistPackage("clihostsample", PublishOutputDir.Combine(CliHostSampleAppProjectName))
        {
            Format = DistPackageFormat.Zip
        }
    ];

    public DirectoryPath DistOutputPath => ArtifactsDir.Combine("dist");
}
