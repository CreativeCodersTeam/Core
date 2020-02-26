using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;

namespace CreativeCoders.NukeBuild.BuildActions
{
    public class CleanBuildAction : BuildActionBase<CleanBuildAction>
    {
        protected override void OnExecute()
        {
            BuildInfo.SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(SafeDeleteDirectory);
            FileSystemTasks.EnsureCleanDirectory(BuildInfo.ArtifactsDirectory);
        }

        private static void SafeDeleteDirectory(AbsolutePath directory)
        {
            FileSystemTasks.EnsureCleanDirectory(directory);
            FileSystemTasks.DeleteDirectory(directory);
        }
    }
}