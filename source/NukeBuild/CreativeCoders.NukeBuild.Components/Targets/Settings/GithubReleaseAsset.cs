using JetBrains.Annotations;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

[PublicAPI]
public class GithubReleaseAsset
{
    public GithubReleaseAsset(string fileName, Stream data)
    {
        FileName = fileName;
    }

    public string FileName { get; }
}
