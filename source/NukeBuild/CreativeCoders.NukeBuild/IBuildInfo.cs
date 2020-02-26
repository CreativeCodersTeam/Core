using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;

namespace CreativeCoders.NukeBuild
{
    [PublicAPI]
    public interface IBuildInfo
    {
        Configuration Configuration { get; }

        Solution Solution { get; }

        GitRepository GitRepository { get; }

        IVersionInfo VersionInfo { get; }

        AbsolutePath SourceDirectory { get; }

        AbsolutePath ArtifactsDirectory { get; }
    }
}