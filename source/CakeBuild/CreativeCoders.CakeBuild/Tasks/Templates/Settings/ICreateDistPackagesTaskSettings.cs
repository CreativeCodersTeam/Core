using Cake.Core.IO;

namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

public interface ICreateDistPackagesTaskSettings : IBuildContextAccessor
{
    IEnumerable<DistPackage> DistPackages => [];

    DirectoryPath DistOutputPath => Context.ArtifactsDir.Combine("dist");
}
