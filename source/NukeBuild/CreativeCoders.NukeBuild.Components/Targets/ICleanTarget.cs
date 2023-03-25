using System.Linq;
using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.NukeBuild.Components.Parameters;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.Components.Targets;

public interface ICleanTarget : INukeBuild
{
    Target Clean => _ => _
        .Before<IRestoreTarget>()
        .Executes(() =>
        {
            var settings = new CleanTargetSettings()
                .Apply(ConfigureCleanSettings);

            DotNetTasks.DotNetClean(settings);

            settings.DirectoriesToClean.SafeDeleteDirectories();
        });

    CleanTargetSettings ConfigureCleanSettings(CleanTargetSettings cleanSettings)
        => ConfigureDefaultCleanSettings(cleanSettings);

    sealed CleanTargetSettings ConfigureDefaultCleanSettings(CleanTargetSettings cleanSettings)
    {
        var settings = cleanSettings.When(this.TryAs<ISolutionParameter>(out var solutionParameter), x => x
            .SetProject(solutionParameter!.Solution));

        if (!this.TryAs<ISourceDirectoryParameter>(out var sourceDirectoryParameter))
        {
            return settings;
        }

        return sourceDirectoryParameter
            .SourceDirectory
            .GlobDirectories("**/bin", "**/obj")
            .Aggregate(settings, (currentSettings, path) => currentSettings.AddDirectoryForClean(path));;
    }
}
