using JetBrains.Annotations;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

[PublicAPI]
public class GithubReleaseAsset
{
    public GithubReleaseAsset(string fileName, Stream data)
    {
        FileName = fileName;
        Data = data;
    }

    public string FileName { get; }

    public Stream Data { get; }

    public bool DisposeStreamAfterUse { get; set; } = true;
}
