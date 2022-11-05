using System.Diagnostics.CodeAnalysis;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.BuildActions;

[ExcludeFromCodeCoverage]
public class RestoreBuildAction : BuildActionBase<RestoreBuildAction>
{
    protected override void OnExecute()
    {
        DotNetTasks.DotNetRestore(s => s
            .SetProjectFile(BuildInfo.Solution));
    }
}
