using JetBrains.Annotations;
using Nuke.Common.IO;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

[PublicAPI]
public class DistPackage(string name, AbsolutePath distFolder)
{
    public DistPackageFormat Format { get; set; }

    public string Name { get; } = name;

    public AbsolutePath DistFolder { get; } = distFolder;
}
