using CreativeCoders.Core;
using Nuke.Common;
using Nuke.Common.IO;

namespace CreativeCoders.NukeBuild.Components.Targets.Settings;

public interface IPackSettings : INukeBuild
{
    AbsolutePath OutputDirectory => this.TryAs<IArtifactsSettings>(out var artifactsSettings)
        ? artifactsSettings.ArtifactsDirectory / "nuget"
        : TemporaryDirectory / "nuget";

    string Copyright => string.Empty;

    string PackageProjectUrl => string.Empty;

    string PackageLicenseUrl => string.Empty;

    string PackageLicenseExpression => string.Empty;
}
