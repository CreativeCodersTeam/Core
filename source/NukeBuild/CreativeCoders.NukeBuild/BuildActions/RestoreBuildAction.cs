using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.BuildActions;

[ExcludeFromCodeCoverage]
[PublicAPI]
public class RestoreBuildAction : BuildActionBase<RestoreBuildAction>
{
    protected override void OnExecute()
    {
        DotNetTasks.DotNetRestore(s => s
            .SetProjectFile(BuildInfo.Solution));
    }
}
