using CreativeCoders.Core;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

public interface IPackSettings
{
    string OutputDirectory => this.TryAs<IArtifactsSettings>(out var artifactsSettings)
        ? artifactsSettings.ArtifactsDirectory / "nuget"
        : string.Empty;

    string Copyright => $"{DateTime.Now.Year} CreativeCoders";
}
