using Nuke.Common.Tooling;

namespace CreativeCoders.NukeBuild.Components;

public class ConfigurableNukeBuild : Nuke.Common.NukeBuild, INukeTargetConfigurator
{
    private IDictionary<Type, Configure<object>> _configures = new Dictionary<Type, Configure<object>>();

    public INukeTargetConfigurator ConfigureTargetSettings<TSettings>(Configure<TSettings> configure)
    {
        _configures[typeof(TSettings)] = x =>
        {
            var settings = (TSettings) x;
            return configure(settings);
        };

        return this;
    }
}
