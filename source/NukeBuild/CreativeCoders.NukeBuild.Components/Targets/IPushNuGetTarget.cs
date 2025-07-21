using CreativeCoders.Core.Collections;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.Components.Targets;

[PublicAPI]
public interface IPushNuGetTarget : IPackTarget, IPushNuGetSettings
{
    Target PushNuGet => d => d
        .OnlyWhenDynamic(() => !SkipPush)
        .DependsOn<IPackTarget>()
        .Executes(() =>
        {
            OutputDirectory
                .GlobFiles("*.nupkg")
                .ForEach(packagePath =>
                {
                    DotNetTasks.DotNetNuGetPush(x => x
                        .Apply(ConfigureNuGetPushSettings, packagePath));
                });
        });

    DotNetNuGetPushSettings ConfigureNuGetPushSettings(DotNetNuGetPushSettings nuGetPushSettings,
        AbsolutePath packagePath)
        => ConfigureDefaultNuGetPushSettings(nuGetPushSettings, packagePath);

    sealed DotNetNuGetPushSettings ConfigureDefaultNuGetPushSettings(
        DotNetNuGetPushSettings nuGetPushSettings, AbsolutePath packagePath)
    {
        return nuGetPushSettings
            .SetTargetPath(packagePath)
            .When(_ => !string.IsNullOrWhiteSpace(NuGetFeedUrl), x => x
                .SetSource(NuGetFeedUrl))
            .When(_ => !string.IsNullOrWhiteSpace(NuGetApiKey), x => x
                .SetApiKey(NuGetApiKey));
    }
}
