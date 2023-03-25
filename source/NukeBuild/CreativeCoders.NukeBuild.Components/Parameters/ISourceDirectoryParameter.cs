using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.IO;

namespace CreativeCoders.NukeBuild.Components.Parameters;

[PublicAPI]
public interface ISourceDirectoryParameter : INukeBuild
{
    [SuppressMessage("Usage", "CA2211")]
    static string SourceDirectoryRelativePath = "source";

    AbsolutePath SourceDirectory => RootDirectory / SourceDirectoryRelativePath;
}
