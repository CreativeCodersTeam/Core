using Cake.Core.IO;
using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

[PublicAPI]
public class DistPackage(string name, DirectoryPath distFolder)
{
    public DistPackageFormat Format { get; set; }

    public string Name { get; } = name;

    public DirectoryPath DistFolder { get; } = distFolder;
}
