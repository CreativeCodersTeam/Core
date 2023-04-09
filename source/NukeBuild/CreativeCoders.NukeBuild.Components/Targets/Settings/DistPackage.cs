using JetBrains.Annotations;
using Nuke.Common.IO;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

[PublicAPI]
public class DistPackage
{
    public DistPackage(string name, AbsolutePath distFolder)
    {
        Name = name;
        DistFolder = distFolder;
    }

    public DistPackageFormat Format { get; set; }

    public string Name { get; }

    public AbsolutePath DistFolder { get; }
}
