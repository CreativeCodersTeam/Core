using Cake.Common.Diagnostics;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Test;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

public class TestTask<T> : FrostingTaskBase<T> where T : BuildContext
{
    protected override Task RunAsyncCore(T context)
    {
        var testSettings = context.GetSettings<ITestTaskSettings>();

        var testProjects = testSettings.TestProjects.OrderBy(x => x.FullPath).ToArray();

        context.Information($"Found {testProjects.Length} test project(s)");

        foreach (var testProject in testProjects)
        {
            context.Information($"Test project found: {testProject.GetFilename()}");

            context.DotNetTest(testProject.FullPath, CreateDotNetBuildSettings(context));
        }

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
