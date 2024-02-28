using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;

namespace CreativeCoders.NukeBuild.BuildActions;

[ExcludeFromCodeCoverage]
[PublicAPI]
public class CleanBuildAction : BuildActionBase<CleanBuildAction>
{
    private readonly List<AbsolutePath> _cleanDirectories = [];

    protected override void OnExecute()
    {
        BuildInfo.SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(SafeDeleteDirectory);

        _cleanDirectories.ForEach(SafeDeleteDirectory);
    }

    private static void SafeDeleteDirectory(AbsolutePath directory)
    {
        directory.CreateOrCleanDirectory();
        directory.DeleteDirectory();
    }

    public CleanBuildAction AddDirectoryForClean(AbsolutePath directory)
    {
        _cleanDirectories.Add(directory);

        return this;
    }
}
