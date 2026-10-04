using Cake.Common.Diagnostics;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Test;
using Cake.Core;
using Cake.Core.IO;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

[PublicAPI]
public class TestTask<T> : FrostingTaskBase<T, ITestTaskSettings> where T : CakeBuildContext
{
    protected override Task RunAsyncCore(T context, ITestTaskSettings taskSettings)
    {
        var testProjects = taskSettings.TestProjects.OrderBy(x => x.FullPath).ToArray();

        context.Information($"Found {testProjects.Length} test project(s)");

        context.Information(taskSettings.UseMicrosoftTestingPlatform
            ? "Test platform: Microsoft Testing Platform"
            : "Test platform: VSTest");

        foreach (var testProject in testProjects)
        {
            context.Information($"Test project found: {testProject.GetFilename()}");

            context.DotNetTest(testProject.FullPath,
                CreateDotNetBuildSettings(context, testProject, taskSettings));
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Customizes the <c>dotnet test</c> settings for a single test project before the test run starts.
    /// </summary>
    /// <param name="context">The build context.</param>
    /// <param name="dotNetBuildSettings">The <c>dotnet test</c> settings to customize.</param>
    /// <remarks>
    /// The hook is called after the base settings (configuration, build, results directory and, in VSTest mode,
    /// loggers and collectors) have been applied. If <see cref="ITestTaskSettings.UseMicrosoftTestingPlatform" /> is
    /// <see langword="true" />, the Microsoft.Testing.Platform arguments are appended after the hook has run; an
    /// <see cref="Cake.Core.Tooling.ToolSettings.ArgumentCustomization" /> set by the hook is kept and runs before
    /// these arguments. In this mode, do not set <see cref="DotNetTestSettings.Loggers" /> or
    /// <see cref="DotNetTestSettings.Collectors" />, because <c>dotnet test</c> does not support them.
    /// </remarks>
    protected virtual void ApplyDotNetTestSettings(T context, DotNetTestSettings dotNetBuildSettings) { }

    private DotNetTestSettings CreateDotNetBuildSettings(T context, FilePath testProject,
        ITestTaskSettings testSettings)
    {
        var dotNetTestSettings = new DotNetTestSettings
        {
            Configuration = context.BuildConfiguration,
            NoBuild = context.HasExecutedTask(typeof(BuildTask<T>)),
            ResultsDirectory = context.CodeCoverageDir
        };

        if (testSettings.UseMicrosoftTestingPlatform)
        {
            dotNetTestSettings.PathType = DotNetTestPathType.Project;
        }
        else
        {
            var testResultFile =
                context.TestResultsDir.CombineWithFilePath($"{testProject.GetFilenameWithoutExtension()}.trx");

            dotNetTestSettings.Loggers = [$"trx;LogFileName={testResultFile}"];
            dotNetTestSettings.Collectors = testSettings.GenerateCoverageReport ? ["XPlat Code Coverage"] : [];
        }

        ApplyDotNetTestSettings(context, dotNetTestSettings);

        if (testSettings.UseMicrosoftTestingPlatform)
        {
            var customArgumentCustomization = dotNetTestSettings.ArgumentCustomization;

            dotNetTestSettings.ArgumentCustomization = args =>
            {
                args = customArgumentCustomization?.Invoke(args) ?? args;

                args.Append("--report-trx");
                args.Append("--report-trx-filename");
                args.AppendQuoted($"{testProject.GetFilenameWithoutExtension()}.trx");

                if (testSettings.GenerateCoverageReport)
                {
                    args.Append("--coverage");
                    args.Append("--coverage-output-format");
                    args.Append("cobertura");
                    args.Append("--coverage-output");
                    args.AppendQuoted($"{testProject.GetFilenameWithoutExtension()}.cobertura.xml");
                }

                return args;
            };
        }

        if (dotNetTestSettings.NoBuild)
        {
            context.Information("Skip build");
        }

        return dotNetTestSettings;
    }
}
