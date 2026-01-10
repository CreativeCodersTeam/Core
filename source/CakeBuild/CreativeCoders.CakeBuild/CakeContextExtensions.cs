using Cake.Common.Diagnostics;
using Cake.Common.Tools.GitVersion;
using Cake.Core;
using Cake.Frosting;
using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild;

[PublicAPI]
public static class CakeContextExtensions
{
    public static bool TaskHasRun<TTask>(this BuildContext context)
        where TTask : IFrostingTask
    {
        return context.ExecutedTasks.Any(x => x.GetType() == typeof(TTask));
    }

    public static GitVersion GetGitVersionSafe(this ICakeContext context,
        string? major = null, string? minor = null, string? patch = null, string? build = null,
        GitVersionSettings? gitVersionSettings = null)
    {
        try
        {
            return gitVersionSettings == null
                ? context.GitVersion()
                : context.GitVersion(gitVersionSettings);
        }
        catch (Exception e)
        {
            context.Warning("GitVersion failed. Using fallback version. Reason: {0}", e.Message);

            return StaticGitVersion.Create(major ?? "0", minor ?? "0", patch ?? "0", build ?? "1", "");
        }
    }
}
