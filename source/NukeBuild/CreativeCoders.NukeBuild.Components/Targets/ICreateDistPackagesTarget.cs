using CreativeCoders.Core.Collections;
using CreativeCoders.IO.Archives;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using JetBrains.Annotations;
using Nuke.Common;

namespace CreativeCoders.NukeBuild.Components.Targets;

[PublicAPI]
public interface ICreateDistPackagesTarget : INukeBuild, ICreateDistPackagesSettings
{
    Target CreateDistPackages => d => d
        .TryAfter<IPublishTarget>()
        .Executes(() =>
        {
            DistPackages.ForEach(distPackage =>
            {
                switch (distPackage.Format)
                {
                    case DistPackageFormat.TarGz:
                        new TarArchiveCreator()
                            .SetArchiveFileName(DistOutputPath / $"{distPackage.Name}.tar.gz")
                            .AddFromDirectory(distPackage.DistFolder, "*.*", true)
                            .Create(true);
                        break;
                    case DistPackageFormat.Zip:
                        new ZipArchiveCreator()
                            .SetArchiveFileName(DistOutputPath / $"{distPackage.Name}.zip")
                            .AddFromDirectory(distPackage.DistFolder, "*.*", true)
                            .Create();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(distPackage),
                            "Package format not supported");
                }
            });
        });
}
