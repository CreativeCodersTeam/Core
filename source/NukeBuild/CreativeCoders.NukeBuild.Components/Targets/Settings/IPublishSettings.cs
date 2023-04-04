using CreativeCoders.Core;
using Nuke.Common;
using Nuke.Common.IO;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

public interface IPublishSettings : INukeBuild
{
    AbsolutePath PublishOutputPath => this.TryAs<IArtifactsSettings>(out var artifactsSettings)
        ? artifactsSettings.ArtifactsDirectory / "dist"
        : TemporaryDirectory / "dist";

    IEnumerable<PublishingItem> PublishingItems => Array.Empty<PublishingItem>();
}
