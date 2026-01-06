using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Common.Tools.DotNet.Pack;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

public class PackTask<T> : FrostingTaskBase<T>
    where T : BuildContext
{
    protected override Task RunAsyncCore(T context)
    {
        var packSettings = context.GetSettings<IPackTaskSettings>();

        context.DotNetPack(context.SolutionFile.FullPath, CreateDotNetPackSettings(context, packSettings));

        return Task.CompletedTask;
    }

    private DotNetPackSettings CreateDotNetPackSettings(T context, IPackTaskSettings packSettings)
    {
        var dotNetPackSettings = new DotNetPackSettings
        {
            OutputDirectory = packSettings.OutputDirectory,
            Configuration = context.BuildConfiguration,
            NoBuild = context.HasExecutedTask(typeof(BuildTask<T>)),
            IncludeSymbols = true,
            SymbolPackageFormat = "snupkg",

            MSBuildSettings = new DotNetMSBuildSettings
            {
                InformationalVersion = context.Version.InformationalVersion,
                AssemblyVersion = context.Version.AssemblySemVer,
                FileVersion = context.Version.AssemblySemFileVer,
                PackageVersion = context.Version.SemVer,
            }
        };

        dotNetPackSettings.MSBuildSettings.WithProperty("PackageProjectUrl", packSettings.PackageProjectUrl);
        dotNetPackSettings.MSBuildSettings.WithProperty("PackageLicenseExpression",
            packSettings.PackageLicenseExpression);
        dotNetPackSettings.MSBuildSettings.WithProperty("PackageLicenseUrl", packSettings.PackageLicenseUrl);

        return dotNetPackSettings;
    }
}
