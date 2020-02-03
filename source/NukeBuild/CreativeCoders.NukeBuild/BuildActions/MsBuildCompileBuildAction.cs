using JetBrains.Annotations;
using Nuke.Common.Tools.MSBuild;

namespace CreativeCoders.NukeBuild.BuildActions
{
    [PublicAPI]
    public class MsBuildCompileBuildAction : BuildActionBase<MsBuildCompileBuildAction>
    {
        protected override void OnExecute()
        {
            MSBuildTasks.MSBuild(s => s
                .SetProjectFile(BuildInfo.Solution)
                .SetConfiguration(BuildInfo.Configuration)
                .SetAssemblyVersion(BuildInfo.VersionInfo.GetNormalizedAssemblyVersion())
                .SetFileVersion(BuildInfo.VersionInfo.GetNormalizedFileVersion())
                .SetInformationalVersion(BuildInfo.VersionInfo.InformationalVersion)
                .DisableRestore());
        }
    }
}