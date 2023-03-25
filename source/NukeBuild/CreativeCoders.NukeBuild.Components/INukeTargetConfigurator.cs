using Nuke.Common.Tooling;

namespace CreativeCoders.NukeBuild.Components;

public interface INukeTargetConfigurator
{
    INukeTargetConfigurator ConfigureTargetSettings<TSettings>(Configure<TSettings> configure);
}
