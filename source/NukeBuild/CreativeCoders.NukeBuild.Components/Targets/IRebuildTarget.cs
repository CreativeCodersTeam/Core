using Nuke.Common;

namespace CreativeCoders.NukeBuild.Components.Targets;

public interface IRebuildTarget
{
    Target Rebuild => d => d
        .DependsOn<ICleanTarget>()
        .DependsOn<IRestoreTarget>()
        .DependsOn<IBuildTarget>();
}
