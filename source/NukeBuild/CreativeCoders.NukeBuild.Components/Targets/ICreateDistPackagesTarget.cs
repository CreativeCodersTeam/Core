using CreativeCoders.Core.Collections;
using CreativeCoders.IO;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using JetBrains.Annotations;
using Nuke.Common;

namespace CreativeCoders.NukeBuild.Components.Targets;

[PublicAPI]
public interface ICreateDistPackagesTarget : INukeBuild, ICreateDistPackagesSettings
{
    Target CreateDistPackages => _ => _
        .Executes(() =>
        {
            DistPackages.ForEach(x =>
            {
                switch (x.Format)
                {
                    case DistPackageFormat.TarGz:
                        new TarArchiveCreator()
                            .SetArchiveFileName(DistOutputPath / $"{x.Name}.tar.gz")
                            .AddFromDirectory(x.DistFolder, "*.*", true)
                            .Create(true);
                        break;
                    case DistPackageFormat.Zip:
                        new ZipArchiveCreator()
                            .SetArchiveFileName(DistOutputPath / $"{x.Name}.zip")
                            .AddFromDirectory(x.DistFolder, "*.*", true)
                            .Create();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(x.Format), "Package format not supported");
                }
            });
        });
}
