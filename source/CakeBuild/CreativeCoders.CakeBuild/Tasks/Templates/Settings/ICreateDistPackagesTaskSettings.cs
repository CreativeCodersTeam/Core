using Cake.Core.IO;

namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

public interface ICreateDistPackagesTaskSettings
{
    IEnumerable<DistPackage> DistPackages { get; }

    DirectoryPath DistOutputPath { get; }
}
