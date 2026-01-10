using Cake.Common.Diagnostics;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.NuGet.Push;
using CreativeCoders.CakeBuild.Tasks.Templates.Settings;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

public class NuGetPushTask<T> : FrostingTaskBase<T> where T : CakeBuildContext
{
    protected override Task RunAsyncCore(T context)
    {
        var pushSettings = context.GetRequiredSettings<INuGetPushTaskSettings>();

        if (pushSettings.SkipPush)
        {
            context.Information("Skip NuGet push");
            return Task.CompletedTask;
        }

        var packSettings = context.GetRequiredSettings<IPackTaskSettings>();

        var filePath = packSettings.OutputDirectory.GetFilePath("*.nupkg");

        context.DotNetNuGetPush(filePath, CreateNuGetPushSettings(pushSettings));

        return Task.CompletedTask;
    }

    private static DotNetNuGetPushSettings CreateNuGetPushSettings(INuGetPushTaskSettings settings)
    {
        var nugetPushSettings = new DotNetNuGetPushSettings
        {
            Source = settings.NuGetFeedUrl,
            ApiKey = settings.NuGetApiKey,
            SkipDuplicate = true,
            IgnoreSymbols = false
        };

        return nugetPushSettings;
    }
}
