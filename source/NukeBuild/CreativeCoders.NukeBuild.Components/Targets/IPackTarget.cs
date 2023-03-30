using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Core;
using CreativeCoders.NukeBuild.Components.Parameters;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.Components.Targets;

[PublicAPI]
public interface IPackTarget : IPackSettings, ISolutionParameter
{
    Target Pack => _ => _
        .TryDependsOn<ICompileTarget>()
        .Executes(() =>
        {
            DotNetTasks.DotNetPack(s => s
                .Apply(ConfigurePackSettings)
            );
        });

    DotNetPackSettings ConfigurePackSettings(DotNetPackSettings packSettings)
        => ConfigureDefaultPackSettings(packSettings);

    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    DotNetPackSettings ConfigureDefaultPackSettings(DotNetPackSettings packSettings)
    {
        return packSettings
            .SetSymbolPackageFormat(DotNetSymbolPackageFormat.snupkg)
            .SetProject(Solution)
            .WhenNotNull(this as IConfigurationParameter, (x, configurationParameter) => x
                .SetConfiguration(configurationParameter.Configuration))
            .EnableIncludeSymbols()
            .When(!string.IsNullOrWhiteSpace(OutputDirectory), x => x
                .SetOutputDirectory(OutputDirectory))
            .WhenNotNull(this as IGitVersionParameter, (x, gitVersionParameter) => x
                .SetVersion(gitVersionParameter.GitVersion?.NuGetVersionV2))
            .SetNoBuild(SucceededTargets.Contains(this.As<ICompileTarget>()?.Compile))
            .When(!string.IsNullOrWhiteSpace(Copyright), x => x
                .SetCopyright(Copyright))
            .When(!string.IsNullOrWhiteSpace(PackageProjectUrl), x => x
                .SetPackageProjectUrl(PackageProjectUrl))
            .When(!string.IsNullOrWhiteSpace(PackageLicenseUrl), x => x
                .SetPackageLicenseUrl(PackageLicenseUrl))
            .When(!string.IsNullOrWhiteSpace(PackageLicenseExpression), x => x
                .SetProperty("PackageLicenseExpression", PackageLicenseExpression));
    }
}
