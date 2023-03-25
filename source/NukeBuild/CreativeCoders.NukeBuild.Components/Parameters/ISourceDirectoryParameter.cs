using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.IO;

namespace CreativeCoders.NukeBuild.Components.Parameters;

[PublicAPI]
public interface ISourceDirectoryParameter : INukeBuild
{
    string SourceDirectoryRelativePath => "source";

    AbsolutePath SourceDirectory => RootDirectory / SourceDirectoryRelativePath;
}
