using Cake.Core.IO;
using JetBrains.Annotations;

namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

[PublicAPI]
public class DistPackage(string name, DirectoryPath sourceFolder)
{
    public DistPackageFormat Format { get; set; }

    public string Name { get; } = name;

    public DirectoryPath SourceFolder { get; } = sourceFolder;
}
