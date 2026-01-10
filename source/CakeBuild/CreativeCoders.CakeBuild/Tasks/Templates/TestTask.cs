using Cake.Common.Diagnostics;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Test;
using Cake.Core.IO;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

[PublicAPI]
public class TestTask<T> : FrostingTaskBase<T> where T : CakeBuildContext
{
    protected override Task RunAsyncCore(T context)
    {
        var testSettings = context.GetRequiredSettings<ITestTaskSettings>();

        var testProjects = testSettings.TestProjects.OrderBy(x => x.FullPath).ToArray();

        context.Information($"Found {testProjects.Length} test project(s)");

        foreach (var testProject in testProjects)
        {
            context.Information($"Test project found: {testProject.GetFilename()}");

            context.DotNetTest(testProject.FullPath,
                CreateDotNetBuildSettings(context, testProject, testSettings));
        }

        return Task.CompletedTask;
    }

    protected virtual void ApplyDotNetTestSettings(T context, DotNetTestSettings dotNetBuildSettings) { }

    private DotNetTestSettings CreateDotNetBuildSettings(T context, FilePath testProject,
        ITestTaskSettings testSettings)
    {
        var testResultFile =
            context.TestResultsDir.CombineWithFilePath($"{testProject.GetFilenameWithoutExtension()}.trx");

        var dotNetTestSettings = new DotNetTestSettings
        {
            Configuration = context.BuildConfiguration,
            NoBuild = context.HasExecutedTask(typeof(BuildTask<T>)),
            Loggers = [$"trx;LogFileName={testResultFile}"],
            Collectors = testSettings.GenerateCoverageReport ? ["XPlat Code Coverage"] : [],
            ResultsDirectory = context.CodeCoverageDir
        };

        ApplyDotNetTestSettings(context, dotNetTestSettings);

        if (dotNetTestSettings.NoBuild)
        {
            context.Information("Skip build");
        }

        return dotNetTestSettings;
    }
}
