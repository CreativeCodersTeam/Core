using CreativeCoders.Core;
using CreativeCoders.NukeBuild.Components.Parameters;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

namespace CreativeCoders.NukeBuild.Components.Targets;

[PublicAPI]
public interface ICompileTarget : ISolutionParameter
{
    Target Compile => d => d
        .TryBefore<ITestTarget>()
        .Executes(() => DotNetTasks.DotNetBuild(x => x
            .Apply(ConfigureCompileSettings)
        ));

    DotNetBuildSettings ConfigureCompileSettings(DotNetBuildSettings buildSettings)
        => ConfigureDefaultCompileSettings(buildSettings);

    sealed DotNetBuildSettings ConfigureDefaultCompileSettings(DotNetBuildSettings buildSettings)
    {
        return buildSettings
            .When(this.TryAs<ISolutionParameter>(out var solutionParameter), x => x
                .SetProjectFile(solutionParameter!.Solution))
            .WhenNotNull(this as IConfigurationParameter, (x, configuration) => x
                .SetConfiguration(configuration.Configuration))
            .WhenNotNull((this as IGitVersionParameter)?.GitVersion, (x, gitVersion) => x
                .SetAssemblyVersion(gitVersion.AssemblySemVer)
                .SetFileVersion(gitVersion.AssemblySemFileVer)
                .SetInformationalVersion(gitVersion.InformationalVersion))
            .SetNoRestore(SucceededTargets.Contains(this.As<IRestoreTarget>()?.Restore));
    }
}
