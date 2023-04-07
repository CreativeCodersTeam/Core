using CreativeCoders.Core.Collections;
using CreativeCoders.IO;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using Nuke.Common;

namespace CreativeCoders.NukeBuild.Components.Targets;

public interface ICreateDistPackagesTarget : INukeBuild, ICreateDistPackagesSettings
{
    Target CreateDistPackages => _ => _
        .Executes(() =>
        {
            DistPackages.ForEach(x =>
            {
                if (x.Format == DistPackageFormat.TarGz)
                {
                    new TarArchiveCreator()
                        .SetArchiveFileName(DistOutputPath / $"{x.Name}.tar.gz")
                        .AddFromDirectory(x.DistFolder, "*.*", true)
                        .Create(true);
                }
            });
        });
}
