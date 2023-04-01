using Nuke.Common.IO;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

public class PublishingItem
{
    public PublishingItem(AbsolutePath projectPath, AbsolutePath outputPath)
    {
        ProjectPath = projectPath;
        OutputPath = outputPath;
    }

    public AbsolutePath ProjectPath { get; }

    public AbsolutePath OutputPath { get; }

    public string? Runtime { get; set; }

    public bool SelfContained { get; set; }
}
