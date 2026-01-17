using CreativeCoders.CakeBuild.Tasks.Templates.Settings;
using CreativeCoders.Core;
using CreativeCoders.IO.Archives;

namespace CreativeCoders.CakeBuild.Tasks.Templates;

public class
    CreateDistPackagesTask<TBuildContext>(IArchiveWriterFactory archiveWriterFactory)
    : FrostingTaskBase<TBuildContext, ICreateDistPackagesTaskSettings>
    where TBuildContext : CakeBuildContext
{
    private readonly IArchiveWriterFactory _archiveWriterFactory = Ensure.NotNull(archiveWriterFactory);

    protected override async Task RunAsyncCore(TBuildContext context,
        ICreateDistPackagesTaskSettings taskSettings)
    {
        foreach (var distPackage in taskSettings.DistPackages)
        {
            switch (distPackage.Format)
            {
                case DistPackageFormat.TarGz:
                    await CreateArchive(() => _archiveWriterFactory.CreateTarGzWriter(
                            taskSettings.DistOutputPath.CombineWithFilePath($"{distPackage.Name}.tar.gz")
                                .FullPath), distPackage)
                        .ConfigureAwait(false);
                    break;
                case DistPackageFormat.Zip:
                    await CreateArchive(() => _archiveWriterFactory.CreateZipWriter(
                            taskSettings.DistOutputPath.CombineWithFilePath($"{distPackage.Name}.zip")
                                .FullPath), distPackage)
                        .ConfigureAwait(false);
                    break;
                default:
                    throw new InvalidOperationException("Package format not supported");
            }
        }
    }

    private static async Task CreateArchive(Func<IArchiveWriter> archiveWriterFactory,
        DistPackage distPackage)
    {
        var archiveWriter = archiveWriterFactory();
        await using var writer = archiveWriter.ConfigureAwait(false);

        await archiveWriter
            .AddFromDirectoryAsync(distPackage.SourceFolder.FullPath, distPackage.SourceFolder.FullPath)
            .ConfigureAwait(false);
    }

    protected override bool SkipIfNoSettings => true;
}
