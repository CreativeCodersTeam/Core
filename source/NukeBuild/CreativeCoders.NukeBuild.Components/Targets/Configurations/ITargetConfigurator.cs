using Nuke.Common.Tooling;

namespace CreativeCoders.NukeBuild.Components.Targets.Configurations;

public interface ITargetConfigurator<TSettings>
    where TSettings : ToolSettings
{
    TSettings Configure(TSettings settings);
}
