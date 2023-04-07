using Nuke.Common.IO;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

public interface ICreateDistPackagesSettings
{
    IEnumerable<DistPackage> DistPackages { get; }

    AbsolutePath DistOutputPath { get; }
}
