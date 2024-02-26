using JetBrains.Annotations;
using Nuke.Common;

namespace CreativeCoders.NukeBuild.Components.Targets;

[PublicAPI]
public interface IDeployNuGetTarget
{
    Target DeployNuGet => d => d
        .DependsOn<IRebuildTarget>()
        .DependsOn<ICodeCoverageTarget>()
        .DependsOn<IPushNuGetTarget>();
}
