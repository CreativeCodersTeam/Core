using JetBrains.Annotations;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

[PublicAPI]
public class GithubReleaseAsset(string fileName)
{
    public string FileName { get; } = fileName;
}
