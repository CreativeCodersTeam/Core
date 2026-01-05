using Cake.Common.Build;
using Cake.Common.Diagnostics;
using Cake.Core;
using Cake.Frosting;
using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild.BuildServer;

[UsedImplicitly]
public class StartGroupTaskSetup : IFrostingTaskSetup
{
    void IFrostingTaskSetup.Setup(ICakeContext context, ITaskSetupContext info)
    {
        if (!context.GitHubActions().IsRunningOnGitHubActions)
        {
            return;
        }

        context.Information("");
        context.GitHubActions().Commands.StartGroup(info.Task.Name);
    }
}
