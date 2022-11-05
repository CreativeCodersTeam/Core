using CreativeCoders.Mvvm.Wpf;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure.Default;

[PublicAPI]
public static class ServiceCollectionExtensions
{
    public static void SetupDefaultInfrastructure(this IServiceCollection services)
    {
        services.TryAddScoped<IWindowManager, WindowManager>();
        services.TryAddScoped<IViewLocator, ViewLocator>();
        services.TryAddScoped<IViewModelBinder, ViewModelBinder>();
        services.TryAddScoped<IViewModelToViewMappings, ViewModelToViewMappings>();
        services.TryAddScoped<IDataTemplateGenerator, DataTemplateGenerator>();
        services.TryAddScoped<IWindowHelper, WindowHelper>();
        services.TryAddScoped<IViewAttributeInitializer, ViewAttributeInitializer>();
    }
}
