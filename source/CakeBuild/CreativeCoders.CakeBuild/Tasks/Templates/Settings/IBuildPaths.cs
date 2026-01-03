using Cake.Core;
using Cake.Core.IO;

namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

[CakeTaskSettings]
public interface IBuildPaths : ICakeBuildSettings
{
    DirectoryPath RootDir => FindGitRootPath(Context.Environment.WorkingDirectory) ??
                             Context.Environment.WorkingDirectory.GetParent();

    DirectoryPath ArtifactsDir => RootDir.Combine(".artifacts");

    DirectoryPath TestOutputBasePath => RootDir.Combine(".tests");

    DirectoryPath CodeCoverageResultsDir => TestOutputBasePath.Combine("coverage-results");

    DirectoryPath? FindGitRootPath(DirectoryPath? startPath)
    {
        while (true)
        {
            if (startPath == null)
            {
                return null;
            }

            if (Context.FileSystem.Exist(startPath.Combine(".git")))
            {
                return startPath;
            }

            startPath = startPath.GetParent();
        }
    }
}
