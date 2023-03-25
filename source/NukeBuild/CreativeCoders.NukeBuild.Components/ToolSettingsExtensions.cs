using CreativeCoders.Core.Reflection;
using CreativeCoders.NukeBuild.Components.Targets.Configurations;
using Nuke.Common.Tooling;

namespace CreativeCoders.NukeBuild.Components;

public static class ToolSettingsExtensions
{
    public static T WhenNotNull<T, TObject>(this T settings, TObject? instance, Func<T, TObject, T> configure)
        where TObject : class
    {
        return instance != null
            ? configure(settings, instance)
            : settings;
    }

    public static TSettings ConfigureSettings<TSettings>(this TSettings settings)
        where TSettings : ToolSettings
    {
        var targetConfiguratorType = typeof(ITargetConfigurator<TSettings>);

        var configuratorImplTypes = targetConfiguratorType.GetImplementations();

        return configuratorImplTypes
            .Select(Activator.CreateInstance)
            .OfType<ITargetConfigurator<TSettings>?>()
            .Aggregate(settings, (current, configurator) => configurator!.Configure(current));
    }
}
