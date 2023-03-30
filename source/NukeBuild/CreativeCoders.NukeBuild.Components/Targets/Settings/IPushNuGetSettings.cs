using Nuke.Common;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

public interface IPushNuGetSettings : INukeBuild
{
    [Parameter(Name = "NuGetFeedUrl")]
    string NuGetFeedUrl => TryGetValue(() => NuGetFeedUrl) ?? string.Empty;

    [Parameter(Name = "NuGetApiKey")]
    string NuGetApiKey => TryGetValue(() => NuGetApiKey) ?? string.Empty;
}
