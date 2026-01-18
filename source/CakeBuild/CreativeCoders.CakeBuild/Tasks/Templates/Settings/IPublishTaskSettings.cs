using Cake.Core.IO;

namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

public interface IPublishTaskSettings : IBuildContextAccessor
{
    DirectoryPath PublishOutputDir => Context.ArtifactsDir.Combine("published");

    IEnumerable<PublishingItem> PublishingItems => [];
}
