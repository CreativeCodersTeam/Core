using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;

namespace CreativeCoders.NukeBuild.BuildActions;

[ExcludeFromCodeCoverage]
public class CleanBuildAction : BuildActionBase<CleanBuildAction>
{
    private readonly IList<AbsolutePath> _cleanDirectories;

    public CleanBuildAction()
    {
        _cleanDirectories = new List<AbsolutePath>();
    }

    protected override void OnExecute()
    {
        BuildInfo.SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(SafeDeleteDirectory);

        _cleanDirectories.ForEach(SafeDeleteDirectory);
    }

    private static void SafeDeleteDirectory(AbsolutePath directory)
    {
        FileSystemTasks.EnsureCleanDirectory(directory);
        FileSystemTasks.DeleteDirectory(directory);
    }

    public CleanBuildAction AddDirectoryForClean(AbsolutePath directory)
    {
        _cleanDirectories.Add(directory);

        return this;
    }
}
