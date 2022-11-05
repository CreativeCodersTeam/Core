using System;

namespace CreativeCoders.Mvvm.Skeletor;

#nullable enable

public static class SkeletorServiceLocator
{
    private static IServiceProvider? ServiceProvider;

    public static void Init(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }
    public static T GetRequiredService<T>()
    {
        return (T) (ServiceProvider?.GetService(typeof(T)) ?? throw new InvalidOperationException());
    }
}
