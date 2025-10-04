using CreativeCoders.Core.SysEnvironment;
using JetBrains.Annotations;
using Nuke.Common.CI.GitHubActions;

namespace CreativeCoders.NukeBuild.Components;

[PublicAPI]
public static class GitHubActionsExtensions
{
    public static bool IsRunnerOs(this GitHubActions gitHubActions, string runnerOs)
    {
        return gitHubActions.GetRunnerOs()
            .Equals(runnerOs, StringComparison.OrdinalIgnoreCase);
    }

    public static string GetRunnerOs(this GitHubActions gitHubActions)
    {
        return Env.GetEnvironmentVariable("RUNNER_OS") ?? string.Empty;
    }

    public static bool IsLocalBuild(this GitHubActions? gitHubActions)
    {
        return gitHubActions == null ||
               Env.GetEnvironmentVariable("GITHUB_ACTIONS")?
                   .Equals("true", StringComparison.OrdinalIgnoreCase) == false;
    }
}
