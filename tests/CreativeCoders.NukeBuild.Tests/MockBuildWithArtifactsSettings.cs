using CreativeCoders.NukeBuild.Components.Targets.Settings;
using Nuke.Common.IO;

namespace CreativeCoders.NukeBuild.Tests;

public class MockBuildWithArtifactsSettings(AbsolutePath artifactsPath) : MockNukeBuild, IArtifactsSettings
{
    AbsolutePath IArtifactsSettings.ArtifactsDirectory => artifactsPath;
}
