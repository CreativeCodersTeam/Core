using Cake.Common.Build;
using Cake.Core;
using Cake.Frosting;
using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild.BuildServer;

[UsedImplicitly]
public class EndGroupTaskTeardown : IFrostingTaskTeardown
{
    void IFrostingTaskTeardown.Teardown(ICakeContext context, ITaskTeardownContext info)
    {
        if (context.GitHubActions().IsRunningOnGitHubActions)
        {
            context.GitHubActions().Commands.EndGroup();
        }
    }
}
