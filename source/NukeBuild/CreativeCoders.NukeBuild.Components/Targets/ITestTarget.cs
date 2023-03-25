using CreativeCoders.NukeBuild.Components.Parameters;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.Components.Targets;

public interface ITestTarget : INukeBuild, ITestTargetSettings, ICompileTarget
{
    Target Test => _ => _
        .After(Compile)
        .Executes(() =>
        {
            DotNetTasks.DotNetTest(x => x
                .Apply(ConfigureTestSettings)
                .CombineWith(TestProjects, ConfigureTestProjectSettings));
        });

    DotNetTestSettings ConfigureTestProjectSettings(DotNetTestSettings settings, Project project)
        => ConfigureDefaultTestProjectSettings(settings, project);

    DotNetTestSettings ConfigureDefaultTestProjectSettings(DotNetTestSettings settings, Project project)
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
            .WhenNotNull(this as IConfigurationParameter, (x, configurationParameter) => x
                .SetConfiguration(configurationParameter.Configuration));
    }
}
