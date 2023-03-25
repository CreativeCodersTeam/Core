using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.NukeBuild.Components.Parameters;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.Components.Targets;

public interface ITestTarget : INukeBuild, ICompileTarget
{
    Target Test => _ => _
        .After(Compile)
        .Executes(() =>
        {
            var settings = new TestTargetSettings()
                .Apply(ConfigureTestSettings);

            if (settings.TestProjectFiles.Any())
            {
                settings.TestProjectFiles.ForEach(testProjectFile =>
                    DotNetTasks.DotNetTest(settings.SetProjectFile(testProjectFile)));
            }
            else
            {
                DotNetTasks.DotNetTest(settings);
            }
        });

    TestTargetSettings ConfigureTestSettings(TestTargetSettings cleanSettings)
        => ConfigureDefaultTestSettings(cleanSettings);

    sealed TestTargetSettings ConfigureDefaultTestSettings(TestTargetSettings cleanSettings)
    {
        var settings = cleanSettings.When(this.TryAs<ISolutionParameter>(out var solutionParameter), x => x
            .SetProjectFile(solutionParameter!.Solution));

        if (!this.TryAs<ISourceDirectoryParameter>(out var sourceDirectoryParameter))
        {
            return settings;
        }

        return settings;
        // return sourceDirectoryParameter
        //     .SourceDirectory
        //     .GlobDirectories("**/bin", "**/obj")
        //     .Aggregate(settings, (currentSettings, path) => currentSettings.AddDirectoryForClean(path));;
    }

    IEnumerable<Project> TestProjects { get; }
}
