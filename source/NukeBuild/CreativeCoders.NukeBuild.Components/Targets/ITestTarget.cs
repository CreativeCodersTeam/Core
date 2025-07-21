using CreativeCoders.Core;
using CreativeCoders.NukeBuild.Components.Parameters;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.Components.Targets;

[PublicAPI]
public interface ITestTarget : ITestSettings
{
    Target Test => d => d
        .TryBefore<ICodeCoverageTarget>()
        .Executes(() =>
        {
            DotNetTasks.DotNetTest(x => x
                .Apply(ConfigureTestSettings)
                .CombineWith(TestProjects, ConfigureTestProjectSettings));
        });

    DotNetTestSettings ConfigureTestProjectSettings(DotNetTestSettings settings, Project project)
        => ConfigureDefaultTestProjectSettings(settings, project);

    sealed DotNetTestSettings ConfigureDefaultTestProjectSettings(DotNetTestSettings settings,
        Project project)
    {
        var testResultFile = Path.Combine(TestResultsDirectory, $"{project.Name}.trx");
        return settings
            .SetProjectFile(project)
            .SetLoggers($"trx;LogFileName={testResultFile}");
    }

    DotNetTestSettings ConfigureTestSettings(DotNetTestSettings testSettings)
        => ConfigureDefaultTestSettings(testSettings);

    sealed DotNetTestSettings ConfigureDefaultTestSettings(DotNetTestSettings testSettings)
    {
        return testSettings
            .SetNoBuild(SucceededTargets.Contains(this.As<IBuildTarget>()?.Build))
            .WhenNotNull(this as IConfigurationParameter, (x, configurationParameter) => x
                .SetConfiguration(configurationParameter.Configuration))
            .When(_ => GenerateCodeCoverage, x => x
                .SetDataCollector("XPlat Code Coverage")
                .SetResultsDirectory(CoverageDirectory));
    }
}
