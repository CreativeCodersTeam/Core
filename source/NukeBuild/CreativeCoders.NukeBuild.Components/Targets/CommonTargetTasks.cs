using CreativeCoders.Core.Collections;
using CreativeCoders.Core.IO;
using JetBrains.Annotations;
using Nuke.Common.IO;

namespace CreativeCoders.NukeBuild.Components.Targets;

[PublicAPI]
public static class CommonTargetTasks
{
    public static void SafeDeleteDirectory(AbsolutePath directory)
    {
        if (!FileSys.Directory.Exists(directory))
        {
            return;
        }

        directory.DeleteDirectory();
    }

    public static void SafeDeleteDirectories(this IEnumerable<AbsolutePath> directories)
    {
        directories.ForEach(SafeDeleteDirectory);
    }
}
