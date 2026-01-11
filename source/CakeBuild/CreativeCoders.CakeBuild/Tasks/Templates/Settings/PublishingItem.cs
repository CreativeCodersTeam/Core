using Cake.Core.IO;

namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

public class PublishingItem(FilePath projectPath, DirectoryPath outputDir)
{
    public FilePath ProjectPath { get; } = projectPath;

    public DirectoryPath OutputDir { get; } = outputDir;

    public string? Runtime { get; set; }

    public bool? SelfContained { get; set; }

    public bool ProduceArtifact { get; set; }
}
