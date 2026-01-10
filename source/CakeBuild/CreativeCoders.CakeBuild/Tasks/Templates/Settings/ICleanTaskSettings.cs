using Cake.Core.IO;

namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

[CakeTaskSettings]
public interface ICleanTaskSettings : IBuildContextAccessor
{
    IList<DirectoryPath> DirectoriesToClean => GetDefaultDirectoriesToClean();

    IList<DirectoryPath> GetDefaultDirectoriesToClean()
    {
        var dirs = new List<DirectoryPath>();

        string[] defaultSourceDirs = ["source", "src", "samples", "tests"];

        var sourceDirs = defaultSourceDirs
            .Select(x => Context.RootDir.Combine(x))
            .Where(Context.FileSystem.Exist)
            .Select(x => x.FullPath);

        foreach (var sourceDir in sourceDirs)
        {
            var binDirs = Context.Globber.Match(sourceDir + "/**/bin")
                .OfType<DirectoryPath>();

            dirs.AddRange(binDirs);

            var objDirs = Context.Globber.Match(sourceDir + "/**/obj")
                .OfType<DirectoryPath>();

            dirs.AddRange(objDirs);
        }

        dirs.AddRange([Context.ArtifactsDir, Context.TestOutputBasePath]);

        return dirs;
    }
}
