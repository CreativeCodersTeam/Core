using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;

namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

[CakeTaskSettings]
public interface ICleanTaskSettings : IBuildContextAccessor
{
    IList<DirectoryPath> DirectoriesToClean => DefaultDirectoriesToClean;

    IList<DirectoryPath> DefaultDirectoriesToClean
    {
        get
        {
            var dirs = new List<DirectoryPath>();

            string[] defaultSourceDirs = ["source", "src", "samples", "tests"];

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

            return dirs;
        }
    }
}
