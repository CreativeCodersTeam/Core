using Nuke.Common;

namespace CreativeCoders.NukeBuild.Components.Targets;

public interface IDeployNuGetTarget
{
    Target DeployNuGet => d => d
        .DependsOn<IRebuildTarget>()
        .DependsOn<ICodeCoverageTarget>()
        .DependsOn<IPushNuGetTarget>();
}
