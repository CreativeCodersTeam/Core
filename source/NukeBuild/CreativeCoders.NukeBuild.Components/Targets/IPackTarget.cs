using System.Diagnostics.CodeAnalysis;
using CreativeCoders.Core;
using CreativeCoders.NukeBuild.Components.Parameters;
using CreativeCoders.NukeBuild.Components.Targets.Settings;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.Components.Targets;

public interface IPackTarget : INukeBuild, ICompileTarget, IPackSettings
{

    Target Pack => _ => _
        .DependsOn<ICompileTarget>()
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
            //.FluentIf(_enableNoBuild, x => x.EnableNoBuild())
            .When(!string.IsNullOrWhiteSpace(Copyright), x => x
                .SetCopyright(Copyright));
            // .FluentIf(!string.IsNullOrWhiteSpace(_copyright), x => x.SetCopyright(_copyright))
            // .FluentIf(!string.IsNullOrWhiteSpace(_packageProjectUrl),
            //     x => x.SetPackageProjectUrl(_packageProjectUrl))
            // .FluentIf(!string.IsNullOrEmpty(_packageLicenseUrl),
            //     x => x.SetPackageLicenseUrl(_packageLicenseUrl))
            // .FluentIf(!string.IsNullOrWhiteSpace(_packageLicenseExpression),
            //     x => x.SetProperty("PackageLicenseExpression", _packageLicenseExpression))
    }
}
