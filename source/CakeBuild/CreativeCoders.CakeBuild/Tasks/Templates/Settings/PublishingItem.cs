using Cake.Core.IO;
using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

[PublicAPI]
public class PublishingItem(FilePath projectPath, DirectoryPath outputDir)
{
    public FilePath ProjectPath { get; } = projectPath;

    public DirectoryPath OutputDir { get; } = outputDir;

    public string? Runtime { get; set; }

    public bool? SelfContained { get; set; }
}
