using CreativeCoders.Core;
using CreativeCoders.Core.Collections;
using CreativeCoders.NukeBuild.Components.Parameters;
using Nuke.Common.IO;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

public interface ICleanTargetSettings
{
    IList<AbsolutePath> DirectoriesToClean => DefaultDirectoriesToClean;

    IList<AbsolutePath> DefaultDirectoriesToClean
    {
        get
        {
            var dirs = new List<AbsolutePath>() as IList<AbsolutePath>;

            if (this.TryAs<ISourceDirectoryParameter>(out var sourceDirectoryParameter))
            {
                dirs.AddRange(sourceDirectoryParameter
                    .SourceDirectory
                    .GlobDirectories("**/bin", "**/obj"));
            }

            return dirs;
        }
    }
}
