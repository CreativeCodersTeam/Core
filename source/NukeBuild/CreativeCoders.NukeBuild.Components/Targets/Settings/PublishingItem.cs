using JetBrains.Annotations;
using Nuke.Common.IO;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

[PublicAPI]
public class PublishingItem(AbsolutePath projectPath, AbsolutePath outputPath)
{
    public AbsolutePath ProjectPath { get; } = projectPath;

    public AbsolutePath OutputPath { get; } = outputPath;

    public string? Runtime { get; set; }

    public bool SelfContained { get; set; }

    public bool ProduceArtifact { get; set; }
}
