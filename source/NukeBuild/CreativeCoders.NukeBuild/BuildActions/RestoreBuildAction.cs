using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.BuildActions;

public class RestoreBuildAction : BuildActionBase<RestoreBuildAction>
{
    protected override void OnExecute()
    {
        DotNetTasks.DotNetRestore(s => s
            .SetProjectFile(BuildInfo.Solution));
    }
}