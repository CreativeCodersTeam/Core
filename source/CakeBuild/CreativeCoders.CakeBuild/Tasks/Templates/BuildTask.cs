using Cake.Common.Diagnostics;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Core.Diagnostics;
using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

[PublicAPI]
public class BuildTask<T> : FrostingTaskBase<T>
    where T : BuildContext
{
    protected virtual void ApplyDotNetBuildSettings(T context, DotNetBuildSettings dotNetBuildSettings) { }

    private DotNetBuildSettings CreateDotNetBuildSettings(T context)
    {
        var dotNetBuildSettings = new DotNetBuildSettings
        {
            Configuration = context.BuildConfiguration,
            NoRestore = context.HasExecutedTask(typeof(RestoreTask<T>)),
            NoIncremental = true,
            MSBuildSettings = new DotNetMSBuildSettings
            {
                InformationalVersion = context.Version.InformationalVersion,
                AssemblyVersion = context.Version.AssemblySemVer,
                FileVersion = context.Version.AssemblySemFileVer
            }
        };

        ApplyDotNetBuildSettings(context, dotNetBuildSettings);

        if (dotNetBuildSettings.NoRestore)
        {
            context.Information("Skip restore");
        }

        return dotNetBuildSettings;
    }

    public override Task RunAsync(T context)
    {
        var dotnetSettings = CreateDotNetBuildSettings(context);

        context.Log.Information("Building solution {0} with configuration {1}",
            context.SolutionFile.FullPath, dotnetSettings.Configuration);

        context.DotNetBuild(context.SolutionFile.FullPath, dotnetSettings);

        return Task.CompletedTask;
    }
}
