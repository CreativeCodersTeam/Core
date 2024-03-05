using CreativeCoders.Core.SysEnvironment;
using JetBrains.Annotations;
using Nuke.Common.CI.GitHubActions;

namespace CreativeCoders.NukeBuild.Components;

[PublicAPI]
public static class GitHubActionsExtensions
{
    public static bool IsRunnerOs(this GitHubActions gitHubActions, string runnerOs)
    {
        return Env.GetEnvironmentVariable("RUNNER_OS")?
            .Equals(runnerOs, StringComparison.OrdinalIgnoreCase) == true;
    }
}
