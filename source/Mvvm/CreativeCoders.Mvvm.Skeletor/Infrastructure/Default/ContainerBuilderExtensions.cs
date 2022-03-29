using CreativeCoders.Di.Building;
using CreativeCoders.Mvvm.Wpf;
using JetBrains.Annotations;

namespace CreativeCoders.Mvvm.Skeletor.Infrastructure.Default;

[PublicAPI]
public static class ContainerBuilderExtensions
{
    public static IDiContainerBuilder SetupDefaultInfrastructure(this IDiContainerBuilder builder)
    {
        builder.AddScoped<IWindowManager, WindowManager>();
        builder.AddScoped<IViewLocator, ViewLocator>();
        builder.AddScoped<IViewModelBinder, ViewModelBinder>();
        builder.AddScoped<IViewModelToViewMappings, ViewModelToViewMappings>();
        builder.AddScoped<IDataTemplateGenerator, DataTemplateGenerator>();
        builder.AddScoped<IWindowHelper, WindowHelper>();
        builder.AddScoped<IViewAttributeInitializer, ViewAttributeInitializer>();

        return builder;
    }
}
