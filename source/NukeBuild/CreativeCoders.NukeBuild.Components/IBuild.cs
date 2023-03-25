using CreativeCoders.NukeBuild.Components.Parameters;
using Nuke.Common;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tooling;

namespace CreativeCoders.NukeBuild.Components;

public interface IBuild : INukeBuild, ISolutionParameter, IConfigurationParameter
{
    Target Build => _ => _
        .Executes(() => DotNetTasks.DotNetBuild(x => x
            .SetProjectFile(Solution)
            .Apply(ConfigureBuildSettings)
        ));

    DotNetBuildSettings ConfigureBuildSettings(DotNetBuildSettings buildSettings)
    {
        return buildSettings;
    }
}
