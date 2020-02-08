using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.BuildActions
{
    public class DotNetCompileBuildAction : BuildActionBase<DotNetCompileBuildAction>
    {
        protected override void OnExecute()
        {
            DotNetTasks.DotNetBuild(s => s
                .SetProjectFile(BuildInfo.Solution)
                .SetConfiguration(BuildInfo.Configuration)
                .SetAssemblyVersion(BuildInfo.VersionInfo.GetNormalizedAssemblyVersion())
                .SetFileVersion(BuildInfo.VersionInfo.GetNormalizedFileVersion())
                .SetInformationalVersion(BuildInfo.VersionInfo.InformationalVersion)
                .EnableNoRestore());
        }
    }
}