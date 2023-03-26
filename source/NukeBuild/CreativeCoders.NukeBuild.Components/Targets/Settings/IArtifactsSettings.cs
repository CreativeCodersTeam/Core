using Nuke.Common;
using Nuke.Common.IO;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

public interface IArtifactsSettings : INukeBuild
{
    AbsolutePath ArtifactsDirectory => RootDirectory / ".artifacts";
}
