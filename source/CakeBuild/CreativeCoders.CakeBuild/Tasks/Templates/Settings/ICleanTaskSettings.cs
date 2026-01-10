using Cake.Core.IO;

namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

[CakeTaskSettings]
public interface ICleanTaskSettings : IBuildContextAccessor
{
    IList<DirectoryPath> DirectoriesToClean => GetDefaultDirectoriesToClean();

    IList<DirectoryPath> GetDefaultDirectoriesToClean()
    {
        var dirs = new List<DirectoryPath>();

        string[] defaultSourceDirs = ["source", "src", "tests"];

        foreach (var defaultSourceDir in defaultSourceDirs)
        {
            var sourceDir = Context.RootDir.Combine(defaultSourceDir);

            if (Context.FileSystem.Exist(sourceDir))
            {
                var binDirs = Context.Globber.Match(sourceDir.FullPath + "/**/bin")
                    .OfType<DirectoryPath>();

                dirs.AddRange(binDirs);

                var objDirs = Context.Globber.Match(sourceDir.FullPath + "/**/obj")
                    .OfType<DirectoryPath>();

                dirs.AddRange(objDirs);
            }
        }

        dirs.AddRange([Context.ArtifactsDir, Context.TestOutputBasePath]);

        return dirs;
    }
}
