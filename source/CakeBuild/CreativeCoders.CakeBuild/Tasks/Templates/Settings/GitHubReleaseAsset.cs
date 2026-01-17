using CreativeCoders.Core;
using CreativeCoders.Core.IO;

namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

public abstract class GitHubReleaseAsset(string assetFileName, string? contentType = null)
{
    public string AssetFileName { get; } = Ensure.IsNotNullOrWhitespace(assetFileName);

    public string ContentType { get; } = contentType ?? MimeMapping.MimeUtility.GetMimeMapping(assetFileName);

    public abstract Stream GetAssetStream();
}

public class GitHubReleaseFileAsset(string fileName, string? assetFileName)
    : GitHubReleaseAsset(assetFileName ?? FileSys.Path.GetFileName(fileName),
        MimeMapping.MimeUtility.GetMimeMapping(fileName))
{
    public override Stream GetAssetStream()
    {
        return FileSys.File.OpenRead(fileName);
    }
}
