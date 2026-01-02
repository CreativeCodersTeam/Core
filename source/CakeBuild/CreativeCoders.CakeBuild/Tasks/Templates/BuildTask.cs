using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Build;
using Cake.Core.Diagnostics;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

public class BuildTask<T>(BuildTaskSettings settings) : FrostingTaskBase<T>
    where T : BuildContext
{
    protected virtual void ApplyDotNetBuildSettings(DotNetBuildSettings dotNetBuildSettings) { }

    private DotNetBuildSettings CreateDotNetBuildSettings()
    {
        var dotNetBuildSettings = new DotNetBuildSettings
        {
            Configuration = settings.Configuration
        };

        ApplyDotNetBuildSettings(dotNetBuildSettings);

        return dotNetBuildSettings;
    }

    public override Task RunAsync(T context)
    {
        var dotnetSettings = CreateDotNetBuildSettings();

        context.Log.Information("Building solution {0} with configuration {1}",
            context.SolutionFile.FullPath, dotnetSettings.Configuration);

        context.DotNetBuild(context.SolutionFile.FullPath, dotnetSettings);

        return Task.CompletedTask;
    }
}

public class BuildTaskSettings
{
    public string? Configuration { get; set; }
}
