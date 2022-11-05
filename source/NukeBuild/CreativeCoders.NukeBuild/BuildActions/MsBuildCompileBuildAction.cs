using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Nuke.Common.Tools.MSBuild;

namespace CreativeCoders.NukeBuild.BuildActions;

[ExcludeFromCodeCoverage]
[PublicAPI]
public class MsBuildCompileBuildAction : BuildActionBase<MsBuildCompileBuildAction>
{
    protected override void OnExecute()
    {
        MSBuildTasks.MSBuild(s => s
            .SetProjectFile(BuildInfo.Solution)
            .SetConfiguration(BuildInfo.Configuration)
            .SetAssemblyVersion(BuildInfo.VersionInfo.GetAssemblySemVer())
            .SetFileVersion(BuildInfo.VersionInfo.GetAssemblySemFileVer())
            .SetInformationalVersion(BuildInfo.VersionInfo.InformationalVersion)
            .DisableRestore());
    }
}
