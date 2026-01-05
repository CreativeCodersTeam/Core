using Cake.Common.Diagnostics;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Test;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

public class TestTask<T> : FrostingTaskBase<T> where T : BuildContext
{
    protected override Task RunAsyncCore(T context)
    {
        context.DotNetTest(context.RootDir.FullPath, CreateDotNetBuildSettings(context));

        return Task.CompletedTask;
    }

    protected virtual void ApplyDotNetTestSettings(T context, DotNetTestSettings dotNetBuildSettings) { }

    private DotNetTestSettings CreateDotNetBuildSettings(T context)
    {
        var dotNetTestSettings = new DotNetTestSettings
        {
            Configuration = context.BuildConfiguration,
            NoBuild = context.HasExecutedTask(typeof(BuildTask<T>))
        };

        ApplyDotNetTestSettings(context, dotNetTestSettings);

        if (dotNetTestSettings.NoBuild)
        {
            context.Information("Skip build");
        }

        return dotNetTestSettings;
    }
}
