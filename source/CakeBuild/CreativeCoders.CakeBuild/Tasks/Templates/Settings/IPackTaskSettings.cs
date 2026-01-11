using Cake.Core.IO;

namespace CreativeCoders.CakeBuild.Tasks.Templates.Settings;

[CakeTaskSettings]
public interface IPackTaskSettings : IBuildContextAccessor
{
    DirectoryPath OutputDirectory => Context.ArtifactsDir.Combine("nuget");

    string Copyright => string.Empty;

    string PackageProjectUrl => string.Empty;

    string PackageLicenseUrl => string.Empty;

    string PackageLicenseExpression => string.Empty;
}
