using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using CreativeCoders.Core.Collections;
using CreativeCoders.IO.Archives;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

public class
    CreateDistPackagesTask<TBuildContext> : FrostingTaskBase<TBuildContext, ICreateDistPackagesTaskSettings>
    where TBuildContext : CakeBuildContext
{
    protected override Task RunAsyncCore(TBuildContext context, ICreateDistPackagesTaskSettings taskSettings)
    {
        foreach (var distPackage in taskSettings.DistPackages)
        {
            switch (distPackage.Format)
            {
                case DistPackageFormat.TarGz:
                    new TarArchiveCreator()
                        .SetArchiveFileName(taskSettings.DistOutputPath
                            .CombineWithFilePath($"{distPackage.Name}.tar.gz").FullPath)
                        .AddFromDirectory(distPackage.DistFolder.FullPath, "*.*", true)
                        .Create(true);
                    break;
                case DistPackageFormat.Zip:
                    new ZipArchiveCreator()
                        .SetArchiveFileName(taskSettings.DistOutputPath
                            .CombineWithFilePath($"{distPackage.Name}.zip").FullPath)
                        .AddFromDirectory(distPackage.DistFolder.FullPath, "*.*", true)
                        .Create();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(distPackage),
                        "Package format not supported");
            }
        }

        return Task.CompletedTask;
    }
}
