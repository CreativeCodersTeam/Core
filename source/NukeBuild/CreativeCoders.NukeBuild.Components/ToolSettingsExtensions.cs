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
}
