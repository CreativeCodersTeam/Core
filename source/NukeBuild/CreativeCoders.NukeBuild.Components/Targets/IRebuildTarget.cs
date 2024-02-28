using JetBrains.Annotations;
using Nuke.Common;

namespace CreativeCoders.NukeBuild.Components.Targets;

[PublicAPI]
public interface IRebuildTarget
{
    Target Rebuild => d => d
        .DependsOn<ICleanTarget>()
        .DependsOn<IRestoreTarget>()
        .DependsOn<IBuildTarget>();
}
